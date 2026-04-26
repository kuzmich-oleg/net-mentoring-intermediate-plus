using Riok.Mapperly.Abstractions;
using TicketingSystem.Common.Extensions;
using TicketingSystem.Domain.Models;
using TicketingSystem.WebAPI.Models.Events;

namespace TicketingSystem.WebAPI.Models.Mappers;

[Mapper]
public static partial class EventsMapper
{
    public static partial EventResponse ToResponse(Event eventModel);

    [UserMapping(Default = true)]
    private static VenueShortInfoModel? MapVenueToShortInfo(Venue? venue)
        => venue.MapIfNotNull(VenuesMapper.ToShortInfo);
}
