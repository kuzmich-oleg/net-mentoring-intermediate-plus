namespace TicketingSystem.WebAPI.Models.Orders;

public sealed record CartResponse
{
    public Guid Id { get; set; }

    public CartItemResponse[] Items { get; init; } = [];

    public decimal TotalPrice { get; set; }
}
