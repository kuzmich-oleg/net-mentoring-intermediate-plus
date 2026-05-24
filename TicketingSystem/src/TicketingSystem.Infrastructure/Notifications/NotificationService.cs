using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TicketingSystem.Application.Interfaces.Services;
using TicketingSystem.Common.Events;
using TicketingSystem.Common.Configurations;

namespace TicketingSystem.Infrastructure.Notifications;

internal sealed class NotificationService : INotificationService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ServiceBusConfiguration _serviceBusConfig;

    public NotificationService(
        ServiceBusClient serviceBusClient,
        IOptions<ServiceBusConfiguration> serviceBusConfig)
    {
        _serviceBusClient = serviceBusClient;
        _serviceBusConfig = serviceBusConfig.Value;
    }

    public async Task PublishEventAsync<T>(T @event, EventType eventType, CancellationToken cancellationToken)
        where T : BaseEvent
    {
        var messageBody = JsonSerializer.Serialize(@event);

        var message = new ServiceBusMessage(messageBody)
        {
            MessageId = @event.EventId.ToString(),
            Subject = @event.OperationName,
            ApplicationProperties = { [nameof(eventType).ToLowerInvariant()] = eventType.ToString() }
        };

        await using var sender = _serviceBusClient.CreateSender(_serviceBusConfig.TopicName);

        await sender.SendMessageAsync(message, cancellationToken);
    }
}
