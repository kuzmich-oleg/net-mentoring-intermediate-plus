namespace TicketingSystem.Common.Extensions;

public static class MappingExtensions
{
    public static TResult? MapIfNotNull<TSource, TResult>(this TSource? source, Func<TSource, TResult> mapFunc)
        where TSource : class
        where TResult : class
        => source == null ? null : mapFunc(source);

    public static List<TResult> MapToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> mapFunc)
        where TSource : class
        where TResult : class
        => [.. source.Select(mapFunc)];

    public static TResult[] MapToArray<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> mapFunc)
        where TSource : class
        where TResult : class
        => [.. source.Select(mapFunc)];
}
