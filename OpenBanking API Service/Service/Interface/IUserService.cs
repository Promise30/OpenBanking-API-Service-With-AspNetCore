using OpenBanking_API_Service.Dtos.AuthenticationDtos.Requests;
using OpenBanking_API_Service.Dtos.AuthenticationDtos.Responses;

namespace OpenBanking_API_Service.Service.Interface
{
    public interface IUserService
    {
        Task<APIResponse<object>> RegisterNewUser(RegisterUserDto registerUser);
        Task<APIResponse<object>> LoginUserAsync(LoginDto loginDto);
        Task<APIResponse<TokenDto>> LoginTwoFactorUserAsync(TwoFactorModel twoFactorModel);
        Task<APIResponse<object>> PasswordResetAsync(ResetPassword resetPassword);
        Task<APIResponse<string>> ForgotPasswordRequest(ForgotPassword forgotPassword);
        Task<APIResponse<object>> UserEmailConfirmation(string token, string email);
        Task<APIResponse<TokenDto>> CreateToken(bool populateExp);
        Task<APIResponse<TokenDto>> RefreshToken(TokenDto token);
        Task<APIResponse<object>> LogoutUserAsync();
        Task<object> GetUsers();

    }
}
