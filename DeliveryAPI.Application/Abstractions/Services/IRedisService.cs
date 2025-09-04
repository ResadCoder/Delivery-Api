namespace DeliveryAPI.Application.Abstractions;

public interface IRedisService
{
    Task<bool> SetAsync(string key, string value, CancellationToken cancellationToken, TimeSpan? expiry = null);
    Task<T?> GetAsync<T>(string key);
    Task<bool> RemoveAsync(string key);
    Task<bool> ExistsAsync(string key);

    Task<bool> SetIfNotExistsAsync(string key, string value, TimeSpan? expiry = null);
    
    Task<T?> TryGetAsync<T>(string key);
}