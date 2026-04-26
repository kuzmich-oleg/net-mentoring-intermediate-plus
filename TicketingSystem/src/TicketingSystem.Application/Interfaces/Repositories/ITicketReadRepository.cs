using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface ITicketReadRepository
{
    Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
