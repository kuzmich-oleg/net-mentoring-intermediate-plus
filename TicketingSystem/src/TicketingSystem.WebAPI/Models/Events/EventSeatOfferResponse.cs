using TicketingSystem.Domain.Models;

namespace TicketingSystem.WebAPI.Models.Events;

public class EventSeatOfferResponse
{
    public Guid OfferId { get; set; }

    public required EventSeatResponse Seat { get; set; }

    public decimal Price { get; set; }

    public SeatStatus Status { get; set; }

    public required string StatusDescription { get; set; }

    public required string SeatPriceLevel { get; set; }
}
