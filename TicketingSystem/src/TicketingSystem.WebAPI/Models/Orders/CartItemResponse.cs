namespace TicketingSystem.WebAPI.Models.Orders;

public sealed record CartItemResponse
{
    public Guid OfferId { get; set; }

    public Guid EventId { get; set; }

    public required CartItemSeatResponse Seat { get; set; }
}
