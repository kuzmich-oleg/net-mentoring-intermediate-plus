using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface ISeatReadRepository
{
    Task<Seat?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
