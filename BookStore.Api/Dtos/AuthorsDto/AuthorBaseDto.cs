using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Dtos.Author
{
    public class AuthorBaseDto
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string? Biography { get; set; }
        public DateOnly BirthDate { get; set; }
    }
}
