using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface IPaymentReadRepository
{
    Task<Payment?> GetByIdAsync(Guid paymentId, CancellationToken cancellationToken);
}
