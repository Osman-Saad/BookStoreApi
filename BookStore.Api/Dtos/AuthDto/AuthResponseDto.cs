using BookStore.Api.UsersDto.Dtos;

namespace BookStore.Api.Dtos.AuthDto
{
    public class AuthResponseDto : UserDto
    {

        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
