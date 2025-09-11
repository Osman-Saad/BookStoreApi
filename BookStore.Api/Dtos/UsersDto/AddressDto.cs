using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Dtos.UsersDto.UserDto
{
    public class AddressDto
    {
        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(200)]
        public string Street { get; set; }
        [MaxLength(50)]
        public string BuildingNumber { get; set; }
    }
}
