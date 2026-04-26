namespace TicketingSystem.Domain.Models;

public sealed record Cart : DomainModelBase
{
    public Guid CustomerId { get; set; }

    public CartStatus Status { get; set; }

    public IList<CartItem> Items { get; set; } = [];

    public decimal TotalPrice => Items.Sum(x => x.Offer?.Price ?? 0);
}
