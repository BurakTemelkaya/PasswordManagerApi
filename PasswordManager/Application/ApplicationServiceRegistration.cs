using Application.Services.AuthService;
using Application.Services.Users;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using Core.Application.Pipelines.Validation;
using Core.Application.Rules;
using Core.Security.JWT;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Core.Localization.Resource.Yaml.DependencyInjection;
using Core.Security.DependencyInjection;
using Application.Features.Auth.Commands.Register;

namespace Application;

public static class ApplicationServiceRegistration
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services, TokenOptions tokenOptions)
	{
		services.AddAutoMapper(Assembly.GetExecutingAssembly());
		services.AddMediatR(configuration =>
		{
			configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			configuration.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
			configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
			configuration.AddOpenBehavior(typeof(CacheRemovingBehavior<,>));
			configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
			configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
			configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
		});

		services.AddSecurityServices<Guid, Guid, Guid>(tokenOptions);

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

		services.AddScoped<IAuthService, AuthManager>();
		services.AddScoped<IUserService, UserManager>();

		services.AddYamlResourceLocalization();

		return services;
	}

	public static IServiceCollection AddSubClassesOfType(
		this IServiceCollection services,
		Assembly assembly,
		Type type,
		Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null
	)
	{
		var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
		foreach (Type? item in types)
			if (addWithLifeCycle == null)
				services.AddScoped(item);
			else
				addWithLifeCycle(services, type);
		return services;
	}

}
