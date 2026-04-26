using TicketingSystem.DataAccess.Entities.Abstractions;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class CartItemEntity : AuditableEntityBase
{
    public Guid CartId { get; set; }
    
    public Guid OfferId { get; set; }
    
    public OfferEntity? Offer { get; set; }
}
