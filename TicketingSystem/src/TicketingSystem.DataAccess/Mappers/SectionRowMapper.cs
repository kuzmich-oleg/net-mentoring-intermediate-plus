using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class SectionRowMapper
{
    [MapperIgnoreSource(nameof(SectionRowEntity.IsDeleted))]
    [MapperIgnoreTarget(nameof(SectionRowEntity.Section))]
    public static partial SectionRow FromEntity(SectionRowEntity sectionRowEntity);

    [MapperIgnoreTarget(nameof(SectionRowEntity.Seats))]
    public static partial SectionRow FromEntityWithSection(SectionRowEntity sectionRowEntity);

    [UserMapping(Default = true)]
    private static Seat SeatFromEntity(SeatEntity seatEntity)
        => SeatMapper.FromEntity(seatEntity);

    [UserMapping(Default = true)]
    private static Section SectionFromEntity(SectionEntity sectionEntity)
        => SectionMapper.FromEntityWithoutRows(sectionEntity);

    [MapperIgnoreTarget(nameof(SectionRowEntity.IsDeleted))]
    [MapperIgnoreTarget(nameof(SectionRowEntity.Section))]
    public static partial SectionRowEntity ToEntity(SectionRow sectionRowModel);
}
