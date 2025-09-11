using BookStore.Core.Models;

namespace BookStore.Core.Specification.OrderSpec
{
    public class OrderWithIdSpecification : BaseSpecification<Order>
    {
        public OrderWithIdSpecification(int orderId, string userId) : base(O => O.Id == orderId && O.UserId == userId)
        {
            Includes.Add(O => O.Items);
        }
    }
}
