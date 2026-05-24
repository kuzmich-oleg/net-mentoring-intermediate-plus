namespace TicketingSystem.Common.Events;

public sealed record PaymentCompletedEvent : BaseEvent
{
    public override string OperationName => "PaymentCompleted";

    public Guid PaymentId { get; init; }

    public required string CustomerEmail { get; init; }
}
