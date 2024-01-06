using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Dtos;

namespace OpenBanking_API_Service.Service.Interface
{
    public interface IUserService
    {
        Task<APIResponse<object>> RegisterNewUser(RegisterUserDto registerUser);
        Task<APIResponse<LoginResponse>> GetJwtTokenAsync(ApplicationUser user);
        Task<APIResponse<LoginOtpResponse>> LoginUserAsync(LoginDto loginDto);
        Task<APIResponse<LoginResponse>> LoginUserWithTwoFactorEnabled(TwoFactorModel twoFactorModel);
        Task<APIResponse<object>> PasswordResetAsync(ResetPassword resetPassword);
        Task<APIResponse<object>> ForgotPasswordRequest(ForgotPassword forgotPassword);
        Task<APIResponse<object>> UserEmailConfirmation(string token, string email);
    }
}
