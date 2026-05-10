using Microsoft.Extensions.Caching.Hybrid;
using TicketingSystem.Application.Interfaces.Services;

namespace TicketingSystem.Infrastructure.Cache;

internal sealed class CacheService : ICacheService
{
    private const string CacheKeyPrefix = "TicketingSystem";

    private readonly HybridCache _hybridCache;

    public CacheService(HybridCache hybridCache)
    {
        _hybridCache = hybridCache;
    }

    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, ValueTask<T>> factory,
        int? expirationMinutes = null,
        int? localExpirationMinutes = null,
        IEnumerable<string>? tags = null,
        CancellationToken cancellationToken = default)
    {
        var entryOptions = CreateEntryOptions(expirationMinutes, localExpirationMinutes);
        var cacheKey = GetCacheKey(key);

        return await _hybridCache.GetOrCreateAsync(
            cacheKey,
            factory.Invoke,
            entryOptions,
            tags,
            cancellationToken);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        int? expirationMinutes = null,
        int? localExpirationMinutes = null,
        IEnumerable<string>? tags = null,
        CancellationToken cancellationToken = default)
    {
        var entryOptions = CreateEntryOptions(expirationMinutes, localExpirationMinutes);
        var cacheKey = GetCacheKey(key);

        await _hybridCache.SetAsync(
            cacheKey,
            value,
            entryOptions,
            tags,
            cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetCacheKey(key);

        await _hybridCache.RemoveAsync(cacheKey, cancellationToken: cancellationToken);
    }

    public async Task RemoveByTagAsync(string tag, CancellationToken cancellationToken = default)
    {
        await _hybridCache.RemoveByTagAsync(tag, cancellationToken);
    }

    private static string GetCacheKey(string key) => $"{CacheKeyPrefix}_{key}";

    private static HybridCacheEntryOptions? CreateEntryOptions(int? expirationMinutes, int? localExpirationMinutes)
    {
        return expirationMinutes.HasValue && localExpirationMinutes.HasValue
            ? new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(expirationMinutes.Value),
                LocalCacheExpiration = TimeSpan.FromMinutes(localExpirationMinutes.Value)
            }
            : null;
    }
}
