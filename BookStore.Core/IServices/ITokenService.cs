using BookStore.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.IServices
{
    public interface ITokenService
    {
        public Task<string> GetAccessToken(AppUser user, UserManager<AppUser> userManager);
        public RefreshToken GetRefreshToken();  
    }
}
