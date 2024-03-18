using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos.AuthenticationDtos.Requests
{
    public class TwoFactorModel
    {
        [Required(ErrorMessage = "OTP code is required")]
        [StringLength(6, ErrorMessage = "OTP code has to be 6 characters.")]
        public string OTP { get; set; } = null!;

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
