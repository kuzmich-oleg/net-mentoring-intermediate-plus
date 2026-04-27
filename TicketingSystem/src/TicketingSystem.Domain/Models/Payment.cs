namespace TicketingSystem.Domain.Models;

public sealed record Payment : DomainModelBase
{
    public Guid OrderId { get; set; }

    public Guid ExternalId { get; set; }

    public decimal Amount { get; set; }

    public PaymentStatus Status { get; set; }

    public Order? Order { get; set; }
}
