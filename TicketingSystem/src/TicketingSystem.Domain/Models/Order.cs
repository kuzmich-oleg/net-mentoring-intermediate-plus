namespace TicketingSystem.Domain.Models;

public sealed record Order
{
    public Guid CustomerId { get; set; }

    public Guid CartId { get; set; }

    public OrderStatus Status { get; set; }
    
    public Cart? Cart { get; set; }

    public IList<Payment> Payments { get; set; } = [];
}
