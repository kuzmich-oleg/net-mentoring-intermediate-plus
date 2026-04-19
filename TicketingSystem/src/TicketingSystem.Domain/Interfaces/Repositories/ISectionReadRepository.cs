using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface ISectionReadRepository
{
    Task<Section?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
