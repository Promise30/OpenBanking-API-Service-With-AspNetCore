using Microsoft.AspNetCore.Identity;

namespace OpenBanking_API_Service.Dtos
{
    public class LoginOtpResponse
    {
        public string Token { get; set; }
        public IdentityUser User { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
    }
}
