namespace TicketingSystem.WebAPI.Models.Orders;

public sealed record class CartItemSeatResponse
{
    public Guid Id { get; set; }

    public int SeatNumber { get; set; }

    public required string SectionRowCode { get; set; }

    public required string SectionCode { get; set; }
}
