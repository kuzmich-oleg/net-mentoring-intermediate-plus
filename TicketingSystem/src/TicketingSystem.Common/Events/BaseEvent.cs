namespace TicketingSystem.Common.Events;

public record BaseEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();

    public virtual string OperationName => string.Empty;

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    public string[] Params { get; init; } = [];
}
