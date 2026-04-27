using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class CartItemMapper
{
    [MapperIgnoreTarget(nameof(Entities.CartItemEntity.Offer))]
    public static partial CartItemEntity ToEntity(CartItem cartItem);
}
