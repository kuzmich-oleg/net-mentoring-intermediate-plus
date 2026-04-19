using TicketingSystem.DataAccess.Entities.Abstractions;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class VenueEntity : AuditableEntityBase
{
    public required string Name { get; set; }

    public required string Location { get; set; }

    public IList<SectionEntity> Sections { get; set; } = [];
}
