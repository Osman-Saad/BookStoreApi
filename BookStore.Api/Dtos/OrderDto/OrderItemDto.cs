namespace BookStore.Api.Dtos.OrderDto
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPice { get; set; }
    }
}
