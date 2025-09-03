using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Models
{
    public class Book: BaseEntity
    {
        public string Name { get; set; }
        public int PageCount { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; }
        public string? CoverUrl { get; set; }
        public DateOnly PublishDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Author> Authors { get; set; } = new HashSet<Author>();
    }
}
