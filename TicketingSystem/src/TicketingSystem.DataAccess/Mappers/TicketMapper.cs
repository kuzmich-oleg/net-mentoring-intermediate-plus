using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class TicketMapper
{
    [MapperIgnoreSource(nameof(TicketEntity.IsDeleted))]
    public static partial Ticket FromEntity(TicketEntity ticketEntity);

    [MapperIgnoreTarget(nameof(TicketEntity.IsDeleted))]
    public static partial TicketEntity ToEntity(Ticket ticketModel);
}
