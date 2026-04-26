using TicketingSystem.DataAccess.Entities.Abstractions;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class PaymentEntity : AuditableEntityBase
{
    public Guid OrderId { get; set; }

    public Guid ExternalId { get; set; }

    public decimal Amount { get; set; }

    public PaymentStatus Status { get; set; }
}
