using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface IEventReadRepository
{
    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Event>> GetEventsAsync(
        DateTimeOffset? eventDate,
        CancellationToken cancellationToken);
}
