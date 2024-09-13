using HomeWork.Redis.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace HomeWork.Redis.Client.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisRepository : IRedisRepository, IDisposable
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly JsonSerializerOptions _serializerSettings;
        private readonly ILogger<RedisRepository> _logger;
        private readonly RedisConfiguration _redisConfiguration;
        private bool _disposed;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RedisRepository(IOptions<RedisConfiguration> configuration, ILogger<RedisRepository> logger)
        {
            _redisConfiguration = configuration?.Value 
                ?? throw new ArgumentNullException(nameof(IOptions<RedisConfiguration>));

            _connection = ConnectionMultiplexer.Connect(_redisConfiguration.ConnectionString);
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            var database = _connection.GetDatabase();
            var value = await database.StringGetAsync(key);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value!, _serializerSettings) : null;
        }

        public Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                var strValue = ConvertToString(value);

                var database = _connection.GetDatabase();

                return database.StringSetAsync(key, strValue, expiry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis repository set {Key} error.", key);
                throw;
            }
        }

        public Task<bool> DeleteAsync(string key)
        {
            var database = _connection.GetDatabase();
            return database.KeyDeleteAsync(key);
        }

        private string ConvertToString<T>(T value) => value switch
        {
            IConvertible convertible => convertible.ToString(System.Globalization.CultureInfo.InvariantCulture),
            Guid id => id.ToString(),
            _ => JsonSerializer.Serialize(value, _serializerSettings)
        };

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _connection.Close();
                _connection.Dispose();
            }

            _disposed = true;
        }
    }
}
