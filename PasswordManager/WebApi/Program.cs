using Application;
using Core.CrossCuttingConcerns.Exception.WebApi.Extensions;
using Core.CrossCuttingConcerns.Logging.Configurations;
using Core.Security.Encryption;
using Core.Security.JWT;
using Core.Security.WebApi.Swagger.Extensions;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    o.JsonSerializerOptions.MaxDepth = 0;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(
        fileLogConfiguration: builder
        .Configuration.GetSection("SeriLogConfigurations:FileLogConfiguration")
        .Get<FileLogConfiguration>()!,
        tokenOptions: builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>()
			?? throw new InvalidOperationException("TokenOptions section cannot found in configuration.")
		);
builder.Services.AddHttpContextAccessor();

builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddInfrastructureServices();

const string tokenOptionsConfigurationSection = "TokenOptions";
TokenOptions tokenOptions =
	builder.Configuration.GetSection(tokenOptionsConfigurationSection).Get<TokenOptions>()
	?? throw new InvalidOperationException($"\"{tokenOptionsConfigurationSection}\" section cannot found in configuration.");
builder
	.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidIssuer = tokenOptions.Issuer,
			ValidAudience = tokenOptions.Audience,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
		};
	});

builder.Services.AddSwaggerGen(opt =>
{
	opt.AddSecurityDefinition(
		name: "Bearer",
		securityScheme: new OpenApiSecurityScheme
		{
			Name = "Authorization",
			Type = SecuritySchemeType.Http,
			Scheme = "Bearer",
			BearerFormat = "JWT",
			In = ParameterLocation.Header,
			Description =
				"JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer YOUR_TOKEN\". \r\n\r\n"
				+ "`Enter your token in the text input below.`"
		}
	);
	opt.OperationFilter<BearerSecurityRequirementOperationFilter>();
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRateLimiter(options =>
{
    // 1. Politika: Hassas Giriþ Ýþlemleri (Fixed Window)
    options.AddFixedWindowLimiter(policyName: "auth-strict", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueLimit = 0;
    });

    // 2. Politika: Genel Vault Ýþlemleri (Sliding Window)
    options.AddSlidingWindowLimiter(policyName: "vault-sync", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(10);
        opt.SegmentsPerWindow = 5;
        opt.QueueLimit = 2;
    });

    options.AddFixedWindowLimiter(policyName: "register-limit", opt =>
    {
        opt.PermitLimit = 3;
        opt.Window = TimeSpan.FromHours(1);
        opt.QueueLimit = 0;
    });

    // Hata durumunda dönecek yanýt (429 Too Many Requests)
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("user-based", httpContext =>
    {
        // Kullanýcý login olmuþsa ID'sini, olmamýþsa IP'sini anahtar (key) olarak kullan
        var key = httpContext.User.Identity?.IsAuthenticated == true
            ? httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            : httpContext.Connection.RemoteIpAddress?.ToString();

        return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 50,
            Window = TimeSpan.FromMinutes(1)
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(); 
}
else
{
    app.ConfigureCustomExceptionMiddleware();
}
	

app.UseHttpsRedirection();

app.UseCors(
    opt =>
        opt.WithOrigins(app.Configuration.GetSection("WebAPIConfiguration").Get<WebApiConfiguration>()!.AllowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
);

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();
