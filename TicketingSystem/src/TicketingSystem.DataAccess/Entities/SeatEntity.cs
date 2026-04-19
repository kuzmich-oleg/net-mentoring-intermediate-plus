using TicketingSystem.DataAccess.Entities.Abstractions;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class SeatEntity : AuditableEntityBase
{
    public Guid SectionRowId { get; set; }

    public int SeatNumber { get; set; }

    public SeatType Type { get; set; }

    public SectionRowEntity? SectionRow { get; set; }
}
