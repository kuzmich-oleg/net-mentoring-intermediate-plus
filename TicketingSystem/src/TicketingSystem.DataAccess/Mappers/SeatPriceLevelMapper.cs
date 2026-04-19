using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class SeatPriceLevelMapper
{
    [MapperIgnoreSource(nameof(SeatPriceLevelEntity.IsDeleted))]
    public static partial SeatPriceLevelInfo FromEntity(SeatPriceLevelEntity seatPriceLevelEntity);

    [MapperIgnoreTarget(nameof(SeatPriceLevelEntity.IsDeleted))]
    public static partial SeatPriceLevelEntity ToEntity(SeatPriceLevelInfo seatPriceLevelModel);
}
