using TicketingSystem.Common;

namespace TicketingSystem.Application.Services.Events.Models;

public class EventsQueryParams
{
    public string? NamePart { get; init; }

    public DateTimeOffset? EventDate { get; init; }

    public required OffsetPage OffsetPage { get; init; }
}
