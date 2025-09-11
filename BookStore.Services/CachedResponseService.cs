using BookStore.Core.IServices;
using StackExchange.Redis;
using System.Text.Json;

namespace BookStore.Services
{
    public class CachedResponseService : ICachedService
    {
        private readonly IDatabase database;
        public CachedResponseService(IConnectionMultiplexer connection)
        {
            database = connection.GetDatabase();
        }
        public async Task CacheResponse(string key, object response, TimeSpan timeSpan)
        {
            if (response != null)
            {
                var jsonOption = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var responseJson = JsonSerializer.Serialize(response, jsonOption);
                await database.StringSetAsync(key, responseJson, timeSpan);
            }
        }

        public async Task<string> GetCashedResponse(string key) =>
            await database.StringGetAsync(key);
    }
}
