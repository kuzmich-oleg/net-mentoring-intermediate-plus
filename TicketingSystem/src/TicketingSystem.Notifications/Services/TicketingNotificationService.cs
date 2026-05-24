using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TicketingSystem.Common.Configurations;
using TicketingSystem.Common.Events;
using TicketingSystem.Notifications.EmailService;

namespace TicketingSystem.Notifications.Services;

internal sealed class TicketingNotificationService : BackgroundService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ServiceBusConfiguration _serviceBusConfig;
    private readonly IEmailService _emailService;
    private readonly ILogger<TicketingNotificationService> _logger;

    private readonly IReadOnlyDictionary<EventType, Type> _eventTypeMapping = new Dictionary<EventType, Type>
    {
        { EventType.PaymentCompleted, typeof(PaymentCompletedEvent) }
    };

    public TicketingNotificationService(
        ServiceBusClient serviceBusClient,
        IOptions<ServiceBusConfiguration> serviceBusConfig,
        IEmailService emailService,
        ILogger<TicketingNotificationService> logger)
    {
        _serviceBusClient = serviceBusClient;
        _serviceBusConfig = serviceBusConfig.Value;
        _emailService = emailService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ConsumeFromTopicAsync(stoppingToken);
    }

    private async Task ConsumeFromTopicAsync(CancellationToken stoppingToken)
    {
        await using var processor = _serviceBusClient.CreateProcessor(
            _serviceBusConfig.TopicName,
            _serviceBusConfig.SubscriptionName,
            new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 1
            });

        processor.ProcessMessageAsync += ProcessMessageAsync;

        processor.ProcessErrorAsync += args =>
        {
            _logger.LogError(args.Exception, "Error processing message from Service Bus");
            return Task.CompletedTask;
        };

        await processor.StartProcessingAsync(stoppingToken);

        try
        {
            // This line awaits indefinitely until stoppingToken is canceled
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
           _logger.LogInformation("TicketingNotificationService is stopping due to cancellation.");
        }
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        try
        {
            var messageBody = args.Message.Body.ToString();
            var eventType = GetEventTypeFromMessage(args.Message);

            switch (eventType)
            {
                case EventType.PaymentCompleted:
                    await ProccessPaymentCompletedEventAsync(messageBody);
                    break;
                default:
                    _logger.LogWarning("Received unknown or malformed event type: {EventType}", eventType);
                    break;
            }

            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message from Service Bus, MessageId: {MessageId}", args.Message.MessageId);
            
            await args.AbandonMessageAsync(args.Message);
        }
    }

    private async Task ProccessPaymentCompletedEventAsync(string messageBody)
    {
        var @event = JsonSerializer.Deserialize<PaymentCompletedEvent>(messageBody);

        if (@event is null)
        {
            _logger.LogWarning("Failed to deserialize {Type} from message body", nameof(PaymentCompletedEvent));
            return;
        }

        var subject = "Payment Completed";
        var body = $"Payment with ID {@event.PaymentId} has been completed successfully.";

        await _emailService.SendEmailAsync(@event.CustomerEmail, subject, body, CancellationToken.None);
    }

    private static EventType? GetEventTypeFromMessage(ServiceBusReceivedMessage message)
        => message.ApplicationProperties.TryGetValue(nameof(EventType).ToLowerInvariant(), out var eventTypeObj)
            && Enum.TryParse<EventType>(eventTypeObj.ToString(), out var eventType)
                ? eventType
                : null;
}
