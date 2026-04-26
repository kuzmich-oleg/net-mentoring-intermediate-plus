using Riok.Mapperly.Abstractions;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.WebAPI.Models.Mappers;

[Mapper]
public static partial class SectionsMapper
{
    public static partial SectionResponse ToResponse(Section section);

    [UserMapping(Default = true)]
    private static SectionRowResponse MapRawToResponse(SectionRow section)
        => RowsMapper.ToResponseWithoutSeats(section);
}
