using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Dtos.AuthDto
{
    public class ChangePasswordRequestDto
    {
        [MinLength(8)]
        public string OldPassword { get; set; }
        [MinLength(8)]
        public string NewPassword { get; set; }
    }
}
