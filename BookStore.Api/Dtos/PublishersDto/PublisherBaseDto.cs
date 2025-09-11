using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Dtos.Publisher
{
    public class PublisherBaseDto
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(15)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

    }
}
