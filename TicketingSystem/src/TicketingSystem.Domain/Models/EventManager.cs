namespace TicketingSystem.Domain.Models;

public sealed record EventManager : DomainModelBase
{
    public Guid UserId { get; set; }

    public required string FullName { get; set; }

    public User? User { get; set; }
}
