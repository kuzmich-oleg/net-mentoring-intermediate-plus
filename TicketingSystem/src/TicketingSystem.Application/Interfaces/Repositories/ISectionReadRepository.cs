using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface ISectionReadRepository
{
    Task<Section?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
