namespace TicketingSystem.Application.Interfaces.Services;

public interface ICacheService
{
    Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, ValueTask<T>> factory,
        int? expirationMinutes = null,
        int? localExpirationMinutes = null,
        IEnumerable<string>? tags = null,
        CancellationToken cancellationToken = default);

    Task SetAsync<T>(
        string key,
        T value,
        int? expirationMinutes = null,
        int? localExpirationMinutes = null,
        IEnumerable<string>? tags = null,
        CancellationToken cancellationToken = default);

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task RemoveByTagAsync(string tag, CancellationToken cancellationToken = default);
}
