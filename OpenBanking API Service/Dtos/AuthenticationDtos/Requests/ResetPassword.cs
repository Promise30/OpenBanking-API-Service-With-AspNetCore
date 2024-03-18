using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos.AuthenticationDtos.Requests
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;

        [Required]
        [Compare("Password", ErrorMessage = "The password does not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Token is required.")]
        public string Token { get; set; } = string.Empty;
    }
}
