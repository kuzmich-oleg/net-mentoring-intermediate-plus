using Azure.Messaging.ServiceBus;
using Mailjet.Client;
using Microsoft.Extensions.Options;
using Polly;
using TicketingSystem.Common.Configurations;
using TicketingSystem.Common.Configurations.Extensions;
using TicketingSystem.Notifications.EmailService;

namespace TicketingSystem.Notifications.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services.RegisterConfiguration<ServiceBusConfiguration>();
        services.RegisterConfiguration<EmailConfiguration>();

        services.AddSingleton(provider =>
        {
            var serviceBusConfig = provider.GetRequiredService<IOptions<ServiceBusConfiguration>>().Value;

            return new ServiceBusClient(serviceBusConfig.ConnectionString);
        });

        services.AddHttpClient<IMailjetClient, MailjetClient>((provider, client) =>
        {
            var emailConfig = provider.GetRequiredService<IOptions<EmailConfiguration>>().Value;

            client.SetDefaultSettings();

            client.UseBasicAuthentication(emailConfig.ApiKey, emailConfig.ApiSecret);
        })
        .AddTransientHttpErrorPolicy(policy =>
            policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)));

        services.AddSingleton<IEmailService, EmailService.EmailService>();

        return services;
    }
}
