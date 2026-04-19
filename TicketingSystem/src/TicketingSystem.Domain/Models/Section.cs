namespace TicketingSystem.Domain.Models;

public sealed record Section : DomainModelBase
{
    public Guid VenueId { get; set; }

    public required string Code { get; set; }

    public Venue? Venue { get; set; }

    public IList<SectionRow> Rows { get; set; } = [];
}
