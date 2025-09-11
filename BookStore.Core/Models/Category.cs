namespace BookStore.Core.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}
