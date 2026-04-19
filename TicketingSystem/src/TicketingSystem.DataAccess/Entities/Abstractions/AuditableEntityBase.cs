namespace TicketingSystem.DataAccess.Entities.Abstractions;

internal abstract class AuditableEntityBase : DbEntityBase, IAuditableEntity
{
    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? LastModifiedAt { get; set; }
}
