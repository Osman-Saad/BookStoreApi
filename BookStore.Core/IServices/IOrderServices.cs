using BookStore.Core.Models;

namespace BookStore.Core.IServices
{
    public interface IOrderServices
    {
        public Task<Order?> CreateOrderAsync(string basketId, string userId);

        public Task<Order?> GetOrderForSpecificUser(int orderId, string userId);
        public Task<IReadOnlyList<Order>> GetOrdersForSpecificUser(string userId);
    }
}
