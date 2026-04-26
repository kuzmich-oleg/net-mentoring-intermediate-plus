namespace TicketingSystem.WebAPI.Models.Events;

public sealed record EventResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public DateTimeOffset EventDate { get; set; }

    public VenueShortInfoModel? Venue { get; set; }
}
