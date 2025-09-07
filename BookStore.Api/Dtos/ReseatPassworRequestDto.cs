using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Dtos
{
    public class ReseatPassworRequestDto
    {
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(8)]
        public string Password { get; set; }
    }
}
