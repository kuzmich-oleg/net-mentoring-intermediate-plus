using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Application.Interfaces.Services;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Application.Services.Events.Models;
using TicketingSystem.Common;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Services.Events;

internal sealed class EventQueryService : IEventQueryService
{
    private const string EventsCacheTag = "EventsList";

    private readonly IEventReadRepository _eventReadRepo;
    private readonly IOfferReadRepository _offerReadRepo;
    private readonly ICacheService _cacheService;

    public EventQueryService(
        IEventReadRepository eventReadRepo,
        IOfferReadRepository offerReadRepo,
        ICacheService cacheService)
    {
        _eventReadRepo = eventReadRepo;
        _offerReadRepo = offerReadRepo;
        _cacheService = cacheService;
    }

    public async Task<PagedResult<Event>> GetEventsAsync(
        EventsQueryParams queryParams,
        CancellationToken cancellationToken)
    {
        ValidateQueryParams(queryParams);

        var cacheKey = CacheKeyHelper.GetEventsPageCacheKey(queryParams);
        
        var events = await _cacheService.GetOrCreateAsync(
            cacheKey,
            async cancellation =>
            {
                return await _eventReadRepo.GetEventsAsync(
                    queryParams.NamePart,
                    queryParams.EventDate,
                    queryParams.OffsetPage,
                    cancellation);
            },
            tags: [ EventsCacheTag ],
            cancellationToken: cancellationToken);

        return events;
    }

    public async Task<IReadOnlyCollection<Offer>?> GetEventSeatOffersAsync(Guid eventId, Guid sectionId,
        CancellationToken cancellationToken)
    {
        var @event = await _cacheService.GetOrCreateAsync(
            CacheKeyHelper.GetEventCacheKey(eventId),
            async cancellationToken => await _eventReadRepo
                .GetByIdAsync(eventId, cancellationToken),
            cancellationToken: cancellationToken);

        var section = @event?.Venue?.Sections.FirstOrDefault(s => s.Id == sectionId);

        if (section is null)
            return null;

        var offersCacheKey = CacheKeyHelper.GetSectionOffersCacheKey(eventId, sectionId);

        var offers = await _cacheService.GetOrCreateAsync(
            offersCacheKey,
            async cancellation =>
            {
                return await _offerReadRepo
                    .GetEventOffersAsync(eventId, sectionId, cancellationToken: cancellation);
            },
            cancellationToken: cancellationToken);

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
