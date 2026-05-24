using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TicketingSystem.Application.Interfaces.Services;
using TicketingSystem.Common.Configurations;
using TicketingSystem.Common.Configurations.Extensions;
using TicketingSystem.Infrastructure.Cache;
using TicketingSystem.Infrastructure.Notifications;

namespace TicketingSystem.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const string PostgresCacheConfigKey = "PostgresCache";
    private const string DisabledPostgresCacheKey = "PostgresCache:Disabled";

    private const string HybridCacheExpirationKey = "HybridCache:DefaultEntryOptions:Expiration";
    private const string HybridCacheLocalExpirationKey = "HybridCache:DefaultEntryOptions:LocalCacheExpiration";

    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        AddDistributedPostgresCache(services, configuration);

        AddHybridCache(services, configuration);

        AddServiceBus(services);

        return services;
    }

    private static void AddDistributedPostgresCache(this IServiceCollection services, IConfiguration configuration)
    {
        var isDisabled = configuration.GetValue<bool?>(DisabledPostgresCacheKey);

        if (isDisabled == true)
            return;

        services.AddDistributedPostgresCache(options =>
        {
            options.ConnectionString = configuration.GetConnectionString(PostgresCacheConfigKey);
            
            configuration.GetSection(PostgresCacheConfigKey).Bind(options);
        });
    }

    public static void AddHybridCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHybridCache(options =>
        {
            var expirationMinutes = configuration.GetValue<int>(HybridCacheExpirationKey);
            var localExpirationMinutes = configuration.GetValue<int>(HybridCacheLocalExpirationKey);

            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(expirationMinutes),
                LocalCacheExpiration = TimeSpan.FromMinutes(localExpirationMinutes)
            };
        });

        services.AddScoped<ICacheService, CacheService>();
    }

    public static void AddServiceBus(this IServiceCollection services)
    {
        services.RegisterConfiguration<ServiceBusConfiguration>();

        services.AddScoped<INotificationService, NotificationService>();
        
        services.AddSingleton(provider =>
        {
            var serviceBusConfig = provider.GetRequiredService<IOptions<ServiceBusConfiguration>>().Value;
            
            return new ServiceBusClient(serviceBusConfig.ConnectionString);
        });
    }
}
