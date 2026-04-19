namespace TicketingSystem.DataAccess.Entities.Abstractions;

internal abstract class DbEntityBase
{
    public Guid Id { get; set; }

    public bool IsDeleted { get; set; }
}
