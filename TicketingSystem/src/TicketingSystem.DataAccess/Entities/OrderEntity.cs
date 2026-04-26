using TicketingSystem.DataAccess.Entities.Abstractions;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class OrderEntity : AuditableEntityBase
{
    public Guid CustomerId { get; set; }

    public Guid CartId { get; set; }

    public OrderStatus Status { get; set; }

    public CartEntity? Cart { get; set; }

    public IList<PaymentEntity> Payments { get; set; } = [];
}
