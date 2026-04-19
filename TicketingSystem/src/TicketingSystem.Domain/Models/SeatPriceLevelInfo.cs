namespace TicketingSystem.Domain.Models;

public sealed record SeatPriceLevelInfo : DomainModelBase
{
    public Guid SeatId { get; set; }

    public SeatPriceLevel PriceLevel { get; set; }

    public Seat? Seat { get; set; }
}
