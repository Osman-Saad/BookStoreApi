namespace BookStore.Core.Models
{
    public class Publisher : BaseEntity
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
