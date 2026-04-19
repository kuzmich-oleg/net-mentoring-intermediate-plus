using TicketingSystem.DataAccess.Entities.Abstractions;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class EventManagerEntity : AuditableEntityBase
{
    public Guid UserId { get; set; }

    public required string FullName { get; set; }

    public UserEntity? User { get; set; }
}
