using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class SectionMapper
{
    [MapperIgnoreSource(nameof(SectionEntity.IsDeleted))]
    [MapperIgnoreTarget(nameof(SectionEntity.Venue))]
    public static partial Section FromEntity(SectionEntity sectionEntity);

    [MapperIgnoreTarget(nameof(SectionEntity.Rows))]
    public static partial Section FromEntityWithoutRows(SectionEntity sectionEntity);

    [UserMapping(Default = true)]
    private static SectionRow RowFromEntity(SectionRowEntity rowEntity)
        => SectionRowMapper.FromEntity(rowEntity);

    [MapperIgnoreTarget(nameof(SectionEntity.IsDeleted))]
    [MapperIgnoreTarget(nameof(SectionEntity.Venue))]
    public static partial SectionEntity ToEntity(Section sectionModel);
}
