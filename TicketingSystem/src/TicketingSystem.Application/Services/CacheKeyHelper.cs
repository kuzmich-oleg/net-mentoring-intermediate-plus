using TicketingSystem.Application.Services.Events.Models;

namespace TicketingSystem.Application.Services;

internal static class CacheKeyHelper
{
    public static string GetEventsPageCacheKey(EventsQueryParams queryParams) =>
        $"Events_{queryParams.NamePart}_{queryParams.EventDate}_{queryParams.OffsetPage.PageNumber}" +
        $"_{queryParams.OffsetPage.PageSize}";

    public static string GetEventCacheKey(Guid eventId) => $"Event_{eventId}";

    public static string GetSectionOffersCacheKey(Guid eventId, Guid sectionId) =>
            $"Event_{eventId}_Section_{sectionId}_Offers";
}
