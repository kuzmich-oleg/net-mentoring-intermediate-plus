using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface ICustomerReadRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Customer?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
