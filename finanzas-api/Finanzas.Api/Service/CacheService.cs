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
            try
            {
                var json = JsonSerializer.Serialize(value);
                await _cache.StringSetAsync(key, json, expiry);
            }
            catch (RedisConnectionException)
            {
                // Redis no disponible, continuar sin cachear
            }
            catch (Exception)
            {
                // Otros errores, continuar sin cachear
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var value = await _cache.StringGetAsync(key);
                return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
            }
            catch (RedisConnectionException)
            {
                // Redis no disponible, retornar valor por defecto
                return default;
            }
            catch (Exception)
            {
                // Otros errores, retornar valor por defecto
                return default;
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _cache.KeyDeleteAsync(key);
            }
            catch (RedisConnectionException)
            {
                // Redis no disponible, continuar
            }
            catch (Exception)
            {
                // Otros errores, continuar
            }
        }
    }
}
