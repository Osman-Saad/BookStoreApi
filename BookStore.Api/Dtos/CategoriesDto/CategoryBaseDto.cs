using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Dtos.Category
{
    public class CategoryBaseDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
