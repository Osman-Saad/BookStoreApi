namespace BookStore.Core.Models
{
    public class Basket
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<BasketItem> Items { get; set; } = new HashSet<BasketItem>();

        public Basket(string id)
        {
            this.Id = id;
        }
    }
}
