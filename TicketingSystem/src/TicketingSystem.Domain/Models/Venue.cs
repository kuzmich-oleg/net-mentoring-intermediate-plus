namespace TicketingSystem.Domain.Models;

public sealed record Venue : DomainModelBase
{
    public required string Name { get; set; }

    public required string Location { get; set; }

    public IList<Section> Sections { get; set; } = [];
}
