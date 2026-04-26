using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface ISeatReadRepository
{
    Task<Seat?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
