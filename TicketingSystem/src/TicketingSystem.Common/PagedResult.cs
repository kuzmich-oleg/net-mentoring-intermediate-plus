namespace TicketingSystem.Common;

public class PagedResult<T> where T : class
{
    public int TotalCount { get; init; }

    public int TotalPages { get; init; }

    public OffsetPage OffsetPage { get; init; }

    public IReadOnlyCollection<T> Items { get; init; } = [];

    public PagedResult(int totalCount, OffsetPage offsetPage, IReadOnlyCollection<T> items)
    {
        TotalCount = totalCount;
        OffsetPage = offsetPage;
        Items = items;
        TotalPages = offsetPage.PageSize == 0
            ? 0
            : (int)Math.Ceiling((double)totalCount / offsetPage.PageSize);
    }
}
