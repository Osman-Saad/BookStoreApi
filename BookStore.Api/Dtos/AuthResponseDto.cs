namespace BookStore.Api.Dtos
{
    public class AuthResponseDto : UserDto
    {

        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
