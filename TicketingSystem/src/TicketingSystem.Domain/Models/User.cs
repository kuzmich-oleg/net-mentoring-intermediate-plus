namespace TicketingSystem.Domain.Models;

public sealed record User : DomainModelBase
{
    public UserType Type { get; set; }

    public required string Email { get; set; }
}
