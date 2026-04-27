using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class OrderMapper
{
    [MapperIgnoreTarget(nameof(OrderEntity.Cart))]
    public static partial OrderEntity ToEntity(Order orderModel);

    public static partial Order FromEntity(OrderEntity orderEntity);

    [UserMapping(Default = true)]
    private static Cart MapCartFormEntity(CartEntity cartEntity)
        => CartMapper.FromEntity(cartEntity);
}
