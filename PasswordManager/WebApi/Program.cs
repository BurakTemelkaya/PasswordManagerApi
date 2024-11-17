using Application;
using Core.Security.Encryption;
using Core.Security.JWT;
using Core.Security.WebApi.Swagger.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(
		tokenOptions: builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>()
			?? throw new InvalidOperationException("TokenOptions section cannot found in configuration.")
		);
builder.Services.AddHttpContextAccessor();

builder.Services.AddPersistenceServices(builder.Configuration);

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


builder.Services.AddDistributedMemoryCache();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
