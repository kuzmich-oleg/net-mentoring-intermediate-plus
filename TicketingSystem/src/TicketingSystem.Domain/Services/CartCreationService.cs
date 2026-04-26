using TicketingSystem.Domain.Interfaces.Services;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Services;

internal sealed class CartCreationService : ICartCreationService
{
    // for now it just creates a cart, but it also should validate the offer, provided cart id, update related seats availability and so on.
    // so it is better to keep this logic in a separate service, rather than in the application layer.
    public Cart CreateFromOffer(Guid id, Offer offer, Guid customerId)
    {
        if (offer.SeatStatus != SeatStatus.Available)
            throw new InvalidOperationException("Offer is not available.");

        return new Cart
        {
            Id = id,
            CustomerId = customerId,
            Status = CartStatus.Created,
            Items = [ new() { CartId = id, OfferId = offer.Id } ]
        };
    }
}
