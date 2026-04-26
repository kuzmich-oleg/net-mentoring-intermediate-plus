using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]

internal static partial class CartMapper
{
    public static partial Cart FromEntity(CartEntity cartEntity);

    [UserMapping(Default = true)]
    private static Offer SeatFromEntity(OfferEntity offerEntity)
        => OfferMapper.FromEntity(offerEntity);

    public static partial CartEntity ToEntity(Cart cartModel);

    [MapperIgnoreTarget(nameof(CartItemEntity.Offer))]
    public static partial CartItemEntity FromEntity(CartItem cartItem);
}
