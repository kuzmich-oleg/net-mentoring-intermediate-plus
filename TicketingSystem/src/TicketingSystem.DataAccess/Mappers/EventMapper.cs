using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class EventMapper
{
    [MapperIgnoreSource(nameof(EventEntity.IsDeleted))]
    public static partial Event FromEntity(EventEntity eventEntity);

    [MapperIgnoreTarget(nameof(EventEntity.IsDeleted))]
    [MapperIgnoreTarget(nameof(EventEntity.Venue))]
    public static partial EventEntity ToEntity(Event eventModel);
}
