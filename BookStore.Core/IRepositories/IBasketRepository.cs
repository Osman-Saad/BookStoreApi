using BookStore.Core.Models;

namespace BookStore.Core.IRepositories
{
    public interface IBasketRepository
    {
        public Task<Basket?> CreateOrUpdateBasketAsync(Basket basket);
        public Task<bool> DeleteBasketAsync(string id);
        public Task<Basket?> GetBasketAsync(string id);
    }
}
