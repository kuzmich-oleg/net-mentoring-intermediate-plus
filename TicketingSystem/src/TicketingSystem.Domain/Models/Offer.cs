namespace TicketingSystem.Domain.Models;

public sealed record Offer : DomainModelBase
{
    public Guid EventId { get; set; }

    public Guid SeatId { get; set; }

    public Guid SeatPriceLevelId { get; set; }

    public decimal Price { get; set; }

    public SeatStatus SeatStatus { get; set; }

    public Seat? Seat { get; set; }

    public SeatPriceLevelInfo? SeatPriceLevel { get; set; }
}
