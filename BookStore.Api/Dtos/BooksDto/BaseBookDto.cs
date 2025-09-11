using BookStore.Api.Validation;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Dtos.BooksDto
{
    public class BaseBookDto
    {
        [MaxLength(50)]
        public string Name { get; set; }
        public int PageCount { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public string Language { get; set; }

        [AllowExtention(new string[] { ".png", ".jpg", ".jpeg" })]
        [MaxSizeMB(1)]
        public IFormFile? CoverImage { get; set; }
        public DateOnly PublishDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public int? PublisherId { get; set; }
        public int? CategoryId { get; set; }
        public ICollection<int> AuthorsIds { get; set; } = new HashSet<int>();
    }
}
