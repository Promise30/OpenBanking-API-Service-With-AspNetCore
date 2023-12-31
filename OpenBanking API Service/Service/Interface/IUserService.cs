using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Dtos;
using OpenBanking_API_Service_Common.Library.Models;

namespace OpenBanking_API_Service.Service.Interface
{
    public interface IUserService
    {
        Task<APIResponse<object>> CreateUserWithToken(RegisterUserDto registerUser);
        Task<APIResponse<LoginResponse>> GetJwtTokenAsync(ApplicationUser user);
        Task<APIResponse<LoginOtpResponse>> GetOtpByLoginAsync(LoginDto loginDto);
        Task<APIResponse<LoginResponse>> LogInUserWIthOtpAsync(string otp, string email);
    }
}
