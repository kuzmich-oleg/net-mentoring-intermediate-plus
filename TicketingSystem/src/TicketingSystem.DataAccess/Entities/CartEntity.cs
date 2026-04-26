using TicketingSystem.DataAccess.Entities.Abstractions;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class CartEntity : AuditableEntityBase
{
    public Guid CustomerId { get; set; }

    public CartStatus Status { get; set; }

    public IList<CartItemEntity> Items { get; set; } = [];
}
