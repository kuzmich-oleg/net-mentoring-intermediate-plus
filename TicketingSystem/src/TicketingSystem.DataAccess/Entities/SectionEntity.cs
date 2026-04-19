using TicketingSystem.DataAccess.Entities.Abstractions;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class SectionEntity : AuditableEntityBase
{
    public Guid VenueId { get; set; }

    public required string Code { get; set; }

    public VenueEntity? Venue { get; set; }

    public IList<SectionRowEntity> Rows { get; set; } = [];
}
