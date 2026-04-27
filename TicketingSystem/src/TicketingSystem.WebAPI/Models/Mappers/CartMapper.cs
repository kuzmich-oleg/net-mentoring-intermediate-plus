using Riok.Mapperly.Abstractions;
using TicketingSystem.Domain.Models;
using TicketingSystem.WebAPI.Models.Orders;

namespace TicketingSystem.WebAPI.Models.Mappers;

[Mapper]
public static partial class CartMapper
{
    public static partial CartResponse ToResponse(Cart cartModel);

    [MapProperty(nameof(CartItem.Offer), nameof(CartItemResponse.Seat), Use = nameof(MapRawToResponse))]
    [MapProperty(nameof(CartItem.Offer), nameof(CartItemResponse.EventId), Use = nameof(GetEventId))]
    public static partial CartItemResponse ToResponse(CartItem cartItem);

    private static CartItemSeatResponse MapRawToResponse(Offer offer)
        => CartItemSeatMapper.ToResponse(offer);

    private static Guid GetEventId(Offer offer)
        => offer.EventId;
}
