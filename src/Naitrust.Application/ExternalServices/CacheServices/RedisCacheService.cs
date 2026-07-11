namespace Naitrust.Application.ExternalServices.CacheServices;

public class RedisCacheService : ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task RemoveAsync(string key, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<bool> ExistsAsync(string key, CancellationToken ct = default) =>
        throw new NotImplementedException();

    public Task<long> IncrementRateLimitAsync(string key, TimeSpan window, CancellationToken ct = default) =>
        throw new NotImplementedException();
}
