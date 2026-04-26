namespace TicketingSystem.Domain.Models;

public sealed record CartItem : DomainModelBase
{
    public Guid CartId { get; set; }

    public Guid OfferId { get; set; }

    public Offer? Offer { get; set; }
}
