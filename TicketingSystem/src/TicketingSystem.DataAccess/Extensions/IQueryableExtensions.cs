using TicketingSystem.Common;
using TicketingSystem.DataAccess.Entities.Abstractions;

namespace TicketingSystem.DataAccess.Extensions;

internal static class IQueryableExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, OffsetPage offsetPage)
        where T : DbEntityBase
    {
        return query
            .Skip((offsetPage.PageNumber - 1) * offsetPage.PageSize)
            .Take(offsetPage.PageSize);
    }
}
