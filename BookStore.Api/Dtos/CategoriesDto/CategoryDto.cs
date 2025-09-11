
using BookStore.Core.Models;

namespace BookStore.Api.Dtos.Category
{
    public class CategoryDto : CategoryBaseDto
    {
        public int Id { get; set; }
        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}
