using System;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Finanzas.Api.Service
{
    public class CacheService
    {
        private readonly IDatabase _cache;
        public CacheService(IConnectionMultiplexer redis) => _cache = redis.GetDatabase();

        public async Task SetAsync(string key, object value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _cache.StringSetAsync(key, json, expiry);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _cache.StringGetAsync(key);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public async Task RemoveAsync(string key) => await _cache.KeyDeleteAsync(key);
    }
}
