using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketingSystem.DataAccess.Repositories.Customers;
using TicketingSystem.DataAccess.Repositories.EventManagers;
using TicketingSystem.DataAccess.Repositories.Events;
using TicketingSystem.DataAccess.Repositories.Payments;
using TicketingSystem.DataAccess.Repositories.SectionRows;
using TicketingSystem.DataAccess.Repositories.Sections;
using TicketingSystem.DataAccess.Repositories.SeatPriceLevels;
using TicketingSystem.DataAccess.Repositories.Seats;
using TicketingSystem.DataAccess.Repositories.Offers;
using TicketingSystem.DataAccess.Repositories.Tickets;
using TicketingSystem.DataAccess.Repositories.Users;
using TicketingSystem.DataAccess.Repositories.Venues;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.DataAccess.Repositories.Carts;
using TicketingSystem.DataAccess.Repositories.Orders;

namespace TicketingSystem.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterTicketingDbContext(configuration);
        
        services.AddRepositories();

        return services;
    }

    public static IServiceCollection RegisterTicketingDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(DbConstants.ConnectionStringName);

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddDbContext<TicketingDbContext>(builder => builder
                .UseSqlServer(
                    connectionString,
                    options => options
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, TicketingDbContext.SchemaName)
                        .EnableRetryOnFailure())
#if DEBUG
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
#endif
        );

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();

        services.AddScoped<IEventManagerReadRepository, EventManagerReadRepository>();
        services.AddScoped<IEventManagerWriteRepository, EventManagerWriteRepository>();

        services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
        services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();

        services.AddScoped<IEventReadRepository, EventReadRepository>();
        services.AddScoped<IEventWriteRepository, EventWriteRepository>();

        services.AddScoped<IVenueReadRepository, VenueReadRepository>();
        services.AddScoped<IVenueWriteRepository, VenueWriteRepository>();

        services.AddScoped<ISectionReadRepository, SectionReadRepository>();
        services.AddScoped<ISectionWriteRepository, SectionWriteRepository>();

        services.AddScoped<ISectionRowReadRepository, SectionRowReadRepository>();
        services.AddScoped<ISectionRowWriteRepository, SectionRowWriteRepository>();

        services.AddScoped<ISeatReadRepository, SeatReadRepository>();
        services.AddScoped<ISeatWriteRepository, SeatWriteRepository>();

        services.AddScoped<ISeatPriceLevelReadRepository, SeatPriceLevelReadRepository>();
        services.AddScoped<ISeatPriceLevelWriteRepository, SeatPriceLevelWriteRepository>();

        services.AddScoped<IOfferReadRepository, OfferReadRepository>();
        services.AddScoped<IOfferWriteRepository, OfferWriteRepository>();

        services.AddScoped<ITicketReadRepository, TicketReadRepository>();
        services.AddScoped<ITicketWriteRepository, TicketWriteRepository>();

        services.AddScoped<ICartReadRepository, CartReadRepository>();
        services.AddScoped<ICartWriteRepository, CartWriteRepository>();

        services.AddScoped<IOrderReadRepository, OrderReadRepository>();
        services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
        services.AddScoped<IPaymentReadRepository, PaymentReadRepository>();

        return services;
    }
}
