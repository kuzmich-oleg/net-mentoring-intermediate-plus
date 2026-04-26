using TicketingSystem.Common;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface IEventReadRepository
{
    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<PagedResult<Event>> GetEventsAsync(
        string? namePart,
        DateTimeOffset? eventDate,
        OffsetPage offsetPage,
        CancellationToken cancellationToken);
}
