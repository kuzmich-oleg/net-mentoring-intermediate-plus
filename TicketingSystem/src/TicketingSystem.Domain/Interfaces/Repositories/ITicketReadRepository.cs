using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface ITicketReadRepository
{
    Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
