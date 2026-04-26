using TicketingSystem.Application.Services.Events.Models;
using TicketingSystem.Common;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Services.Queries;

public interface IEventQueryService
{
    Task<PagedResult<Event>> GetEventsAsync(
        EventsQueryParams queryParams,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Offer>?> GetEventSeatOffersAsync(Guid eventId, Guid sectionId,
        CancellationToken cancellationToken);
}
