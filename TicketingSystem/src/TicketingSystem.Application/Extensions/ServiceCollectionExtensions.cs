using Microsoft.Extensions.DependencyInjection;
using TicketingSystem.Application.Interfaces.Services;
using TicketingSystem.Application.Interfaces.Services.Commands;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Application.Services;
using TicketingSystem.Application.Services.Events;
using TicketingSystem.Application.Services.Orders;
using TicketingSystem.Application.Services.Venues;

namespace TicketingSystem.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddApplicationServices();

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentCustomerProvider, CurrentCustomerProvider>();

        services.AddScoped<IVenueQueryService, VenueQueryService>();
        services.AddScoped<IEventQueryService, EventQueryService>();
        services.AddScoped<IOrderQueryService, OrderQueryService>();

        services.AddScoped<IOrderCommandService, OrderCommandService>();

        return services;
    }
}
