using TicketingSystem.DataAccess.Entities.Abstractions;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class EventEntity : AuditableEntityBase
{
    public Guid VenueId { get; set; }

    public required string Name { get; set; }
    
    public required string Description { get; set; }

    public DateTimeOffset EventDate { get; set; }

    public VenueEntity? Venue { get; set; }
}
