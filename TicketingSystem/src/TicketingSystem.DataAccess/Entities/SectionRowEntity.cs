using TicketingSystem.DataAccess.Entities.Abstractions;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class SectionRowEntity : AuditableEntityBase
{
    public Guid SectionId { get; set; }

    public required string Code { get; set; }

    public SectionEntity? Section { get; set; }

    public IList<SeatEntity> Seats { get; set; } = [];
}
