using Riok.Mapperly.Abstractions;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.WebAPI.Models.Mappers;

[Mapper]
public static partial class RowsMapper
{
    public static partial SectionRowResponse ToResponse(SectionRow sectionRow);

    [MapperIgnoreTarget(nameof(SectionRowResponse.Seats))]
    public static partial SectionRowResponse ToResponseWithoutSeats(SectionRow sectionRow);
}
