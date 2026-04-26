using TicketingSystem.Application.Services.Orders.Models;

namespace TicketingSystem.WebAPI.Models.Orders;

public sealed record CartCreationRequest
{
    // Offer is a container for event_id, seat_id and price_id
    public Guid OfferId { get; set; }

    public CreateCartCommand ToCommand(Guid cartId)
    {
        return new CreateCartCommand
        {
            CartId = cartId,
            OfferId = OfferId
        };
    }
}
