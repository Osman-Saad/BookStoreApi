using BookStore.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Core.IServices
{
    public interface ITokenService
    {
        public Task<string> GetAccessToken(AppUser user, UserManager<AppUser> userManager);
        public RefreshToken GetRefreshToken();
    }
}
