using Infrastructure.Caching;
using Infrastructure.Caching.Microsoft;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddSingleton<ICacheManager, MemoryCacheManager>();

        return services;
    }
}
