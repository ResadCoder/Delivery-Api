using System.Text.Json.Serialization;
using DeliveryAPI.Application.Abstractions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DeliveryAPI.Persistence.Implementations.Services;

public class RedisService(IConnectionMultiplexer connectionMultiplexer) : IRedisService
{
    private readonly IDatabase _db = connectionMultiplexer.GetDatabase();

    public async Task<bool> SetAsync(string key, string value,
        CancellationToken cancellationToken, TimeSpan? expiry = null)
    {
        return await _db.StringSetAsync(key, value, expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        string? value = (await _db.StringGetAsync(key));
        if (value == RedisValue.Null)
            throw new Exception($"Redis {key}  not found");
        return JsonConvert.DeserializeObject<T>(value)
            ?? throw new Exception($"Failed to deserialize {key}");
    }

    public async Task<bool> RemoveAsync(string key)
    {
       return await _db.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
       return await _db.KeyExistsAsync(key);
    }

    public async Task<T?> TryGetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        if(!value.HasValue) return default;

        return JsonConvert.DeserializeObject<T>(value.ToString());
    }

    public async Task<bool> SetIfNotExistsAsync(string key, string value, TimeSpan? expiry = null)
    {
        return await _db.StringSetAsync(key, value, expiry, When.NotExists);
    }
}