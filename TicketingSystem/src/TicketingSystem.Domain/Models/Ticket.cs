namespace TicketingSystem.Domain.Models;

public sealed record Ticket : DomainModelBase
{
    public Guid CustomerId { get; set; }

    public Guid EventId { get; set; }

    public Guid OfferId { get; set; }

    public Guid OrderId { get; set; }

    public Event? Event { get; set; }

    public Offer? Offer { get; set; }

    public Order? Order { get; set; }
}
