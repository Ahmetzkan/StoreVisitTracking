using Microsoft.Extensions.Options;
using Moq;
using StackExchange.Redis;
using StoreVisitTracking.Application.Interfaces;
using StoreVisitTracking.Application.Services;
using StoreVisitTracking.Application.Settings;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace StoreVisitTracking.Tests.Services
{
    public class RedisCacheServiceTests : IDisposable
    {
        private readonly Mock<IConnectionMultiplexer> _mockRedis;
        private readonly Mock<IDatabase> _mockDatabase;
        private readonly Mock<IServer> _mockServer;
        private readonly RedisCacheService _cacheService;
        private readonly string _instanceName = "TestInstance_";

        public RedisCacheServiceTests()
        {
            _mockRedis = new Mock<IConnectionMultiplexer>();
            _mockDatabase = new Mock<IDatabase>();
            _mockServer = new Mock<IServer>();

            _mockRedis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_mockDatabase.Object);

            var endpoint = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 6379);
            _mockRedis.Setup(r => r.GetServer(endpoint, It.IsAny<object>()))
                .Returns(_mockServer.Object);

            _mockRedis.Setup(r => r.GetEndPoints(It.IsAny<bool>()))
                .Returns(new[] { endpoint });

            var redisSettings = Options.Create(new RedisSettings
            {
                Configuration = "localhost:6379",
                InstanceName = _instanceName
            });

            _cacheService = new RedisCacheService(redisSettings, _mockRedis.Object);
        }

        public void Dispose()
        {
            _cacheService.Dispose();
        }

        [Fact]
        public async Task GetAsync_WhenKeyExists_ShouldReturnValue()
        {
            var key = "testKey";
            var expectedValue = new TestObject { Id = 1, Name = "Test" };
            var serializedValue = JsonSerializer.Serialize(expectedValue);

            _mockDatabase.Setup(d => d.StringGetAsync(_instanceName + key, It.IsAny<CommandFlags>()))
                .ReturnsAsync(serializedValue);

            var result = await _cacheService.GetAsync<TestObject>(key);

            Assert.NotNull(result);
            Assert.Equal(expectedValue.Id, result.Id);
            Assert.Equal(expectedValue.Name, result.Name);
        }

        [Fact]
        public async Task GetAsync_WhenKeyDoesNotExist_ShouldReturnDefault()
        {
            var key = "nonExistentKey";
            _mockDatabase.Setup(d => d.StringGetAsync(_instanceName + key, It.IsAny<CommandFlags>()))
                .ReturnsAsync(RedisValue.Null);

            var result = await _cacheService.GetAsync<TestObject>(key);

            Assert.Null(result);
        }

        [Fact]
        public async Task SetAsync_ShouldCallStringSetAsync()
        {
            var key = "testKey";
            var value = new TestObject { Id = 1, Name = "Test" };
            var serializedValue = JsonSerializer.Serialize(value);

            _mockDatabase.Setup(d => d.StringSetAsync(
                    _instanceName + key,
                    serializedValue,
                    It.IsAny<TimeSpan?>(),
                    It.IsAny<bool>(),  
                    It.IsAny<When>(),
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            await _cacheService.SetAsync(key, value);

            _mockDatabase.Verify(d => d.StringSetAsync(
                _instanceName + key,
                serializedValue,
                null,
                false,  
                When.Always,
                CommandFlags.None),
            Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_ShouldCallKeyDeleteAsync()
        {
            var key = "testKey";
            _mockDatabase.Setup(d => d.KeyDeleteAsync(_instanceName + key, It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            await _cacheService.RemoveAsync(key);

            _mockDatabase.Verify(d => d.KeyDeleteAsync(_instanceName + key, CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task ExistsAsync_ShouldCallKeyExistsAsync()
        {
            var key = "testKey";
            _mockDatabase.Setup(d => d.KeyExistsAsync(_instanceName + key, It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            var result = await _cacheService.ExistsAsync(key);

            Assert.True(result);
            _mockDatabase.Verify(d => d.KeyExistsAsync(_instanceName + key, CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task RemoveByPrefixAsync_ShouldDeleteAllKeysWithPrefix()
        {
            var prefix = "testPrefix";
            var pattern = _instanceName + prefix + "*";
            var keys = new RedisKey[] { _instanceName + prefix + "1", _instanceName + prefix + "2" };

            _mockServer.Setup(s => s.Keys(
                    It.IsAny<int>(), 
                    pattern,          
                    It.IsAny<int>(),  
                    It.IsAny<long>(), 
                    It.IsAny<int>(),  
                    It.IsAny<CommandFlags>()))
                .Returns(keys);

            _mockDatabase.Setup(d => d.KeyDeleteAsync(keys, It.IsAny<CommandFlags>()))
                .ReturnsAsync(2);

            await _cacheService.RemoveByPrefixAsync(prefix);

            _mockDatabase.Verify(d => d.KeyDeleteAsync(keys, CommandFlags.None), Times.Once);
        }

        private class TestObject
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }
    }
}