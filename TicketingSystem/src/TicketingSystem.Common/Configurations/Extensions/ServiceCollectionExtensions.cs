using Microsoft.Extensions.DependencyInjection;

namespace TicketingSystem.Common.Configurations.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterConfiguration<TConfig>(this IServiceCollection services)
        where TConfig : class, IConfig
    {
        services
            .AddOptions<TConfig>()
            .BindConfiguration(TConfig.SectionName)
            .ValidateOnStart();

        return services;
    }
}
