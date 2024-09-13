namespace HomeWork.Redis.Client.Repository
{
    public interface IRedisRepository
    {
        Task<T?> GetAsync<T>(string key) where T : class;

        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null);

        Task<bool> DeleteAsync(string key);
    }
}
