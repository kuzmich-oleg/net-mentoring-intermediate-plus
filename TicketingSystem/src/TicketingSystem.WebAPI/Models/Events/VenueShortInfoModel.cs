namespace TicketingSystem.WebAPI.Models.Events;

public sealed record VenueShortInfoModel
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Location { get; set; }
}
