using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos.AuthenticationDtos.Requests
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
