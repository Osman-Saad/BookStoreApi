using BookStore.Core.Models;

namespace BookStore.Core.Specification.OrderSpec
{
    public class OrdersByUserIdSpecification : BaseSpecification<Order>
    {
        public OrdersByUserIdSpecification(string userId) : base(O => O.UserId == userId)
        {
            Includes.Add(O => O.Items);
        }
    }
}
