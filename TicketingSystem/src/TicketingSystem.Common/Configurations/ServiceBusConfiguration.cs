namespace TicketingSystem.Common.Configurations;

public sealed class ServiceBusConfiguration : IConfig
{
    public static string SectionName => "ServiceBus";

    public required string ConnectionString { get; init; }

    public required string TopicName { get; init; }

    public required string SubscriptionName { get; init; }
}
