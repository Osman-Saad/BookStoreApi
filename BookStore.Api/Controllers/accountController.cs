using AutoMapper;
using BookStore.Api.Dtos;
using BookStore.Api.Dtos.AuthDto;
using BookStore.Api.Dtos.UsersDto.UserDto;
using BookStore.Api.Errors;
using BookStore.Api.Helpers;
using BookStore.Api.UsersDto.Dtos;
using BookStore.Core;
using BookStore.Core.IServices;
using BookStore.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security.Claims;

namespace BookStore.Api.Controllers
{

    public class accountController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public accountController(
            IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> RegisterAsCustomer([FromBody] RegisterRequestDto registerRequest)
        {
            if (CheckEmail(registerRequest.Email).Result.Value)
                return BadRequest(new ApiResponse(400, "Email is already exist"));
            var user = new AppUser
            {
                Email = registerRequest.Email,
                UserName = new MailAddress(registerRequest.Email).User,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                PhoneNumber = registerRequest.PhoneNumber,
                Address = mapper.Map<UserAddress>(registerRequest.Address)
            };
            var result = await userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, string.Join(",", result.Errors.Select(E => E.Description))));
            if (registerRequest.IsAdmin)
                await userManager.AddToRoleAsync(user, Roles.Admin);
            else
                await userManager.AddToRoleAsync(user, Roles.Customer);
            var accessToken = await tokenService.GetAccessToken(user, userManager);
            var refreshToken = tokenService.GetRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await userManager.UpdateAsync(user);
            var response = new AuthResponseDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = mapper.Map<AddressDto>(user.Address),
                Token = accessToken,
                RefreshToken = refreshToken.Token
            };
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto loginRequest)
        {
            var user = await userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null) return Unauthorized(new ApiResponse(400, "Email Or Password Is Not Correct"));
            var passwordValid = await userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!passwordValid) return Unauthorized(new ApiResponse(400, "Email Or Password Is Not Correct"));
            var refreshToken = tokenService.GetRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await userManager.UpdateAsync(user);
            var response = new AuthResponseDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = mapper.Map<AddressDto>(user.Address),
                Token = await tokenService.GetAccessToken(user, userManager),
                RefreshToken = refreshToken.Token
            };
            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) return NotFound(new ApiResponse(404, "User Not Found"));
            var response = mapper.Map<UserDto>(user);
            return Ok(response);
        }

        [HttpGet("refresh")]
        public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
        {
            Console.WriteLine(refreshToken);
            var user = await userManager.Users
                .Include(U => U.RefreshTokens)
                .FirstOrDefaultAsync(U => U.RefreshTokens.Any(R => R.Token.Trim() == refreshToken.Trim()));
            if (user is null) return Unauthorized(new ApiResponse(401, "Invalid Refresh Tokenm"));
            var token = user.RefreshTokens.FirstOrDefault(R => R.Token == refreshToken);
            if (token is null || !token.IsActive)
                return Unauthorized(new ApiResponse(401, "Invalid Refresh Token"));
            token.RevokeOn = DateTime.UtcNow;
            var accessToken = await tokenService.GetAccessToken(user, userManager);
            var newRefreshToken = tokenService.GetRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await userManager.UpdateAsync(user);
            return Ok(new
            {
                Token = accessToken,
                RefreshToken = newRefreshToken.Token,
            });
        }

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> Logout(string refreshToken)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.FirstOrDefaultAsync(U => U.Email == email);
            var token = user.RefreshTokens.FirstOrDefault(R => R.Token == refreshToken);
            token.RevokeOn = DateTime.UtcNow;
            await userManager.UpdateAsync(user);
            return Ok(new MessageResponse() { Message = "Logout Successfuly" });
        }

        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePasswordRequestDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) return NotFound(new ApiResponse(404, "User Not Found"));
            var result = await userManager.ChangePasswordAsync(user, changePasswordRequestDto.OldPassword, changePasswordRequestDto.NewPassword);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400, string.Join(",", result.Errors.Select(E => E.Description))));
            return Ok(new MessageResponse() { Message = "Passsword Changed Successfully" });
        }

        [HttpDelete("delete-account")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAccount()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) return NotFound(new ApiResponse(404, "User Not Found"));
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400, string.Join(",", result.Errors.Select(E => E.Description))));
            return Ok(new MessageResponse() { Message = "Account Deleted Successfully" });
        }

        [HttpPost("reseat-password")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]

        public async Task<ActionResult> ReseatPassword([FromBody] ReseatPassworRequestDto reseatPassworRequestDto)
        {
            var user = await userManager.FindByEmailAsync(reseatPassworRequestDto.Email);
            if (user is null) return NotFound(new ApiResponse(404, "User Not Found"));
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, reseatPassworRequestDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400, string.Join(",", result.Errors.Select(E => E.Description))));
            return Ok(new MessageResponse() { Message = "Password Reseated Successfully" });
        }

        [HttpPatch("update-profile")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]

        public async Task<ActionResult<UserDto>> UpdateProfile(UserDto userDto)
        {
            var user = await userManager.FindByIdAsync(userDto.UserId);
            if (user is null) return NotFound(new ApiResponse(404, "User Not Found"));
            if (!string.IsNullOrEmpty(userDto.FirstName))
                user.FirstName = userDto.FirstName;
            if (!string.IsNullOrEmpty(userDto.LastName))
                user.LastName = userDto.LastName;
            if (!string.IsNullOrEmpty(userDto.PhoneNumber))
                user.PhoneNumber = userDto.PhoneNumber;
            if (userDto.Address != null)
            {
                if (!string.IsNullOrEmpty(userDto.Address.City))
                    user.Address.City = userDto.Address.City;
                if (!string.IsNullOrEmpty(userDto.Address.Street))
                    user.Address.Street = userDto.Address.Street;
                if (!string.IsNullOrEmpty(userDto.Address.BuildingNumber))
                    user.Address.BuildingNumber = userDto.Address.BuildingNumber;
            }
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400, string.Join(",", result.Errors.Select(E => E.Description))));
            var response = mapper.Map<UserDto>(user);
            return Ok(response);
        }

        [HttpGet("check-email")]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null) return Ok(true);
            return Ok(false);
        }

        [HttpGet("users")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAllUsers()
        {
            var users = await userManager.Users.Include(U => U.Address).ToListAsync();
            var response = mapper.Map<IReadOnlyList<UserDto>>(users);
            return Ok(response);
        }
    }
}
