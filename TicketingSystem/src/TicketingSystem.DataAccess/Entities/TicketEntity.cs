using TicketingSystem.DataAccess.Entities.Abstractions;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class TicketEntity : AuditableEntityBase
{
    public Guid CustomerId { get; set; }

    public Guid OfferId { get; set; }

    public OfferEntity? Offer { get; set; }
}
