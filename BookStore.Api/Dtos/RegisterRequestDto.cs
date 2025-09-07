using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Dtos
{
    public class RegisterRequestDto
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string  PhoneNumber { get; set; }
        [MinLength(8)]
        public string Password { get; set; }
        public AddressDto Address { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
