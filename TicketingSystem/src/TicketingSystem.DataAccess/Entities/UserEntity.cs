using TicketingSystem.DataAccess.Entities.Abstractions;
using TicketingSystem.Domain;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class UserEntity : AuditableEntityBase
{
    public UserType Type { get; set; }

    public required string Email { get; set; }
}
