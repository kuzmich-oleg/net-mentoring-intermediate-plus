namespace TicketingSystem.Domain.Models;

public abstract record DomainModelBase
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? LastModifiedAt { get; set; }
}
