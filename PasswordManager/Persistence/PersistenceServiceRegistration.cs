using Application.Services.Repositories;
using Application.Services.Users;
using Core.Persistence.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BaseDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("PasswordManagerDb")));
        services.AddDbMigrationApplier(buildServices => buildServices.GetRequiredService<BaseDbContext>());

		//services.AddScoped<IEmailAuthenticatorRepository, EmailAuthenticatorRepository>();
		services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
		services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();

		return services;
    }
}
