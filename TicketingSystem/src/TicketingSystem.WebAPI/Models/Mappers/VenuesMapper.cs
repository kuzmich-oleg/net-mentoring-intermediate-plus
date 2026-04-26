using Riok.Mapperly.Abstractions;
using TicketingSystem.Domain.Models;
using TicketingSystem.WebAPI.Models.Events;

namespace TicketingSystem.WebAPI.Models.Mappers;

[Mapper]
public static partial class VenuesMapper
{
    public static partial VenueResponse ToResponse(Venue venue);

    public static partial VenueShortInfoModel ToShortInfo(Venue venue);

    [UserMapping(Default = true)]
    private static SectionResponse MapSectionToResponse(Section section)
        => SectionsMapper.ToResponse(section);
}
