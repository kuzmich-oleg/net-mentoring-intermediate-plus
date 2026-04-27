using Microsoft.Extensions.DependencyInjection;
using TicketingSystem.Domain.Interfaces.Services;
using TicketingSystem.Domain.Services;

namespace TicketingSystem.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDomain(this IServiceCollection services)
    {
        //TODO: consider moving to separate method
        services.AddScoped<IOrdersService, OrdersService>();

        return services;
    }
}
