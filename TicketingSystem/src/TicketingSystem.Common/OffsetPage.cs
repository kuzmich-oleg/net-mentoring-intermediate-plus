namespace TicketingSystem.Common;

public record OffsetPage
{
    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public OffsetPage(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
