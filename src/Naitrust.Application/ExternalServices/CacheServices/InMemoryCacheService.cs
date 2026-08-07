using Microsoft.Extensions.Caching.Memory;

namespace Naitrust.Application.ExternalServices.CacheServices;

public class InMemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public InMemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        _cache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default)
    {
        var options = new MemoryCacheEntryOptions();
        if (expiry.HasValue)
            options.AbsoluteExpirationRelativeToNow = expiry.Value;

        _cache.Set(key, value, options);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken ct = default)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string key, CancellationToken ct = default)
    {
        return Task.FromResult(_cache.TryGetValue(key, out _));
    }

    public Task<long> IncrementRateLimitAsync(string key, TimeSpan window, CancellationToken ct = default)
    {
        var current = _cache.GetOrCreate(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = window;
            return 0L;
        });

        var next = current + 1;
        _cache.Set(key, next, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = window
        });

        return Task.FromResult(next);
    }
}
