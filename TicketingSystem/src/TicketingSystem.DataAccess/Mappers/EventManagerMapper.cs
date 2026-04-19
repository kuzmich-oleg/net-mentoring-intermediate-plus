using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class EventManagerMapper
{
    [MapperIgnoreSource(nameof(EventManagerEntity.IsDeleted))]
    public static partial EventManager FromEntity(EventManagerEntity eventManagerEntity);

    [MapperIgnoreTarget(nameof(EventManagerEntity.IsDeleted))]
    public static partial EventManagerEntity ToEntity(EventManager eventManagerModel);
}
