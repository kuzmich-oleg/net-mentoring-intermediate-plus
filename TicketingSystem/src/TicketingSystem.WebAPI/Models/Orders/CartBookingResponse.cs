namespace TicketingSystem.WebAPI.Models.Orders;

public sealed record CartBookingResponse
{
    public Guid PaymentId { get; set; }
}
