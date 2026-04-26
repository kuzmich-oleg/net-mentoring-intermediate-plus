using TicketingSystem.Application.Services.Events.Models;
using TicketingSystem.Common;
using TicketingSystem.Common.Constants;

namespace TicketingSystem.WebAPI.Models.Events;

public record EventsSearchRequest
{
    public string? NamePart { get; init; } = null;

    public DateTimeOffset? EventDate { get; init; } = null;

    public int PageNumber { get; init; } = EventsSearchConstants.DefaultPageNumber;

    public int PageSize { get; init; } = EventsSearchConstants.DefaultPageSize;

    public EventsQueryParams ToQueryParams()
    {
        return new EventsQueryParams
        {
            NamePart = NamePart,
            EventDate = EventDate,
            OffsetPage = new OffsetPage(PageNumber, PageSize)
        };
    }
}
