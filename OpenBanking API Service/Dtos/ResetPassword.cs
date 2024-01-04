using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password does not match.")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
