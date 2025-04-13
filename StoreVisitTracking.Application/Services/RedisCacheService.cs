using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StoreVisitTracking.Application.Interfaces;
using StoreVisitTracking.Application.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IDatabase = StackExchange.Redis.IDatabase;

namespace StoreVisitTracking.Application.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _database;
        private readonly string _instanceName;
        private readonly IConnectionMultiplexer _redis;
        private readonly IServer _server;

        public RedisCacheService(IOptions<RedisSettings> redisSettings, IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = _redis.GetDatabase();
            _server = _redis.GetServer(_redis.GetEndPoints().First());
            _instanceName = redisSettings.Value.InstanceName;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(_instanceName + key);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(_instanceName + key, serializedValue, expiry);
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(_instanceName + key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _database.KeyExistsAsync(_instanceName + key);
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            var keys = _server.Keys(pattern: _instanceName + prefix + "*").ToArray();
            if (keys.Any())
            {
                await _database.KeyDeleteAsync(keys);
            }
        }

        public void Dispose()
        {
            _redis?.Dispose();
        }
    }
}