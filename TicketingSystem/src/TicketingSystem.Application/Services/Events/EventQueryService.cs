using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Application.Services.Events.Models;
using TicketingSystem.Common;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Services.Events;

internal sealed class EventQueryService : IEventQueryService
{
    private readonly IEventReadRepository _eventReadRepo;
    private readonly IOfferReadRepository _offerReadRepo;

    public EventQueryService(
        IEventReadRepository eventReadRepo,
        IOfferReadRepository offerReadRepo)
    {
        _eventReadRepo = eventReadRepo;
        _offerReadRepo = offerReadRepo;
    }

    public async Task<PagedResult<Event>> GetEventsAsync(
        EventsQueryParams queryParams,
        CancellationToken cancellationToken)
    {
        ValidateQueryParams(queryParams);

        var events = await _eventReadRepo.GetEventsAsync(
            queryParams.NamePart,
            queryParams.EventDate,
            queryParams.OffsetPage,
            cancellationToken);

        return events;
    }

    public async Task<IReadOnlyCollection<Offer>?> GetEventSeatOffersAsync(Guid eventId, Guid sectionId,
        CancellationToken cancellationToken)
    {
        var @event = await _eventReadRepo.GetByIdAsync(eventId, cancellationToken);

        var section = @event?.Venue?.Sections.FirstOrDefault(s => s.Id == sectionId);

        if (section is null)
            return null;

        var offers = await _offerReadRepo.GetEventOffersAsync(eventId, sectionId, cancellationToken);
        return offers;
    }

    private static void ValidateQueryParams(EventsQueryParams queryParams)
    {
        if (queryParams.OffsetPage.PageSize < 0)
        {
            throw new ArgumentException("Page size cannot be negative.", nameof(queryParams));
        }

        if (queryParams.OffsetPage.PageNumber <= 0)
        {
            throw new ArgumentException("Page number must be greater than zero.", nameof(queryParams));
        }
    }
}
