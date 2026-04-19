using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface IEventManagerReadRepository
{
    Task<EventManager?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<EventManager?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
