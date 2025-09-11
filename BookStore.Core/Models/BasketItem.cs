namespace BookStore.Core.Models
{
    public class BasketItem
    {
        public int BooKId { get; set; }
        public string BookName { get; set; }
        public string BookCovverUrl { get; set; }
        public string BookCategory { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
