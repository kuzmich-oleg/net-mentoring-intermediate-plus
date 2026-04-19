namespace TicketingSystem.Domain.Models;

public sealed record Event : DomainModelBase
{
    public Guid VenueId { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public DateTimeOffset EventDate { get; set; }

    public Venue? Venue { get; set; }
}
