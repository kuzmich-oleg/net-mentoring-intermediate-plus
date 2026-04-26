namespace TicketingSystem.WebAPI.Models;

public sealed record VenueResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Location { get; set; }

    public SectionResponse[] Sections { get; set; } = [];
}
