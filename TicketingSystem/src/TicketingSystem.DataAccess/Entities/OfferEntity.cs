using TicketingSystem.DataAccess.Entities.Abstractions;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class OfferEntity : AuditableEntityBase
{
    public Guid EventId { get; set; }

    public Guid SeatId { get; set; }

    public Guid SeatPriceLevelId { get; set; }

    public decimal Price { get; set; }

    public SeatStatus SeatStatus { get; set; }

    public EventEntity? Event { get; set; }

    public SeatEntity? Seat { get; set; }

    public SeatPriceLevelEntity? SeatPriceLevel { get; set; }

    public TicketEntity? Ticket { get; set; }
}
