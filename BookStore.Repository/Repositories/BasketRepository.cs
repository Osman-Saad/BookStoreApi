using BookStore.Core.IRepositories;
using BookStore.Core.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace BookStore.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase Database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            Database = redis.GetDatabase();
        }
        public async Task<Basket?> CreateOrUpdateBasketAsync(Basket basket)
        {
            var basketJson = JsonSerializer.Serialize(basket);
            var createdOrUpdated = await Database.StringSetAsync(basket.Id, basketJson, TimeSpan.FromDays(1));
            if (!createdOrUpdated)
                return null;
            return await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string id) =>
            await Database.KeyDeleteAsync(id);

        public async Task<Basket?> GetBasketAsync(string id)
        {
            var basket = await Database.StringGetAsync(id);
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Basket>(basket);
        }


    }
}
