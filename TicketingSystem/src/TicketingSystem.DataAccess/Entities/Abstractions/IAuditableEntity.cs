namespace TicketingSystem.DataAccess.Entities.Abstractions;

internal interface IAuditableEntity
{
    DateTimeOffset CreatedAt { get; set; }

    DateTimeOffset? LastModifiedAt { get; set; }
}
