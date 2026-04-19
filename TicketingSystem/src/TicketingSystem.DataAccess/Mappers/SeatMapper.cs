using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class SeatMapper
{
    [MapperIgnoreSource(nameof(SeatEntity.IsDeleted))]
    public static partial Seat FromEntity(SeatEntity seatEntity);

    [MapperIgnoreTarget(nameof(SeatEntity.IsDeleted))]
    [MapperIgnoreTarget(nameof(SeatEntity.SectionRow))]
    public static partial SeatEntity ToEntity(Seat seatModel);
}
