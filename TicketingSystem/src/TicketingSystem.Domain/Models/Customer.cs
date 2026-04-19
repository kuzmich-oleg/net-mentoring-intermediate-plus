namespace TicketingSystem.Domain.Models;

public sealed record Customer : DomainModelBase
{
    public Guid UserId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public User? User { get; set; }
}
