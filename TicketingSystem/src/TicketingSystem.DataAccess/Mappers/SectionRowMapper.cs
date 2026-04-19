using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class SectionRowMapper
{
    [MapperIgnoreSource(nameof(SectionRowEntity.IsDeleted))]
    public static partial SectionRow FromEntity(SectionRowEntity sectionRowEntity);

    [MapperIgnoreTarget(nameof(SectionRowEntity.IsDeleted))]
    [MapperIgnoreTarget(nameof(SectionRowEntity.Section))]
    public static partial SectionRowEntity ToEntity(SectionRow sectionRowModel);
}
