using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class OfferMapper
{
    [MapperIgnoreSource(nameof(OfferEntity.IsDeleted))]
    public static partial Offer FromEntity(OfferEntity offerEntity);

    [MapperIgnoreTarget(nameof(OfferEntity.IsDeleted))]
    public static partial OfferEntity ToEntity(Offer offerModel);
}
