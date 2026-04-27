using TicketingSystem.Domain.Models;

namespace TicketingSystem.WebAPI.Models;

public sealed record PaymentResponse
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ExternalId { get; set; }

    public decimal Amount { get; set; }

    public required string Status { get; set; }
}
