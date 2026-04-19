using TicketingSystem.DataAccess.Entities.Abstractions;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class SeatPriceLevelEntity : AuditableEntityBase
{
    public SeatPriceLevel PriceLevel { get; set; }
}
