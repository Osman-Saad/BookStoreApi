using BookStore.Core;
using BookStore.Core.IRepositories;
using BookStore.Core.IServices;
using BookStore.Core.Models;
using BookStore.Core.Specification.BookSpec;
using BookStore.Core.Specification.OrderSpec;

using Order = BookStore.Core.Models.Order;
namespace BookStore.Services
{
    public class OrderService : IOrderServices
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Order?> CreateOrderAsync(string basketId, string userId)
        {
            var basket = await basketRepository.GetBasketAsync(basketId);
            if (basket == null || !basket.Items.Any())
                return null;
            var bookIds = basket.Items.Select(I => I.BooKId).ToList();
            var bookSpec = new BookSpecificationWithIds(bookIds);
            var books = await unitOfWork.Repository<Book>().GetAllAsync(bookSpec);
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var book = books.FirstOrDefault(B => B.Id == item.BooKId);
                if (book == null || book.Quantity < item.Quantity)
                    return null;
                var orderItem = new OrderItem
                {
                    BookId = book.Id,
                    BookName = book.Name,
                    Price = book.Price,
                    Quantity = item.Quantity,
                    CoverUrl = book.CoverUrl ?? string.Empty
                };
                book.Quantity -= item.Quantity;
                orderItems.Add(orderItem);
            }
            var totalPrice = orderItems.Sum(O => O.Price * O.Quantity);
            var order = new Order
            {
                Items = orderItems,
                TotalPrice = totalPrice,
                UserId = userId
            };
            await unitOfWork.Repository<Order>().AddAsync(order);
            var result = await unitOfWork.CompleteAsync();
            basket.Items.Clear();
            await basketRepository.CreateOrUpdateBasketAsync(basket);
            if (result <= 0) return null;
            return order;
        }

        public async Task<Order?> GetOrderForSpecificUser(int orderId, string userId)
        {
            var orderSpec = new OrderWithIdSpecification(orderId, userId);
            return await unitOfWork.Repository<Order>().GetByIdAsync(orderSpec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUser(string userId)
        {
            var orderSpec = new OrdersByUserIdSpecification(userId);
            return await unitOfWork.Repository<Order>().GetAllAsync(orderSpec);
        }
    }
}
