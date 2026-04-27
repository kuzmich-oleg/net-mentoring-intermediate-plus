using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Services.Queries;

public interface IPaymentQueryService
{
    Task<Payment?> GetPaymentAsync(Guid paymentId, CancellationToken cancellationToken);
}
