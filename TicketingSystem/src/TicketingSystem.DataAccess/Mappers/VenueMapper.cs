using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class VenueMapper
{
    [MapperIgnoreSource(nameof(VenueEntity.IsDeleted))]
    public static partial Venue FromEntity(VenueEntity venueEntity);

    [MapperIgnoreTarget(nameof(VenueEntity.IsDeleted))]
    public static partial VenueEntity ToEntity(Venue venueModel);
}
