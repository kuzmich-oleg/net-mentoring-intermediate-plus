using TicketingSystem.DataAccess.Entities.Abstractions;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class CustomerEntity : AuditableEntityBase
{
    public Guid UserId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public UserEntity? User { get; set; }

    public IList<TicketEntity> Tickets { get; set; } = [];

    public IList<CartEntity> Carts { get; set; } = [];

    public IList<OrderEntity> Orders { get; set; } = [];
}
