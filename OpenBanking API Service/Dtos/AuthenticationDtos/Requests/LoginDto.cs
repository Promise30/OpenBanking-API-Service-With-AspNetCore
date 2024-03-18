using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos.AuthenticationDtos.Requests
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password cannot be less than 8 characters")]
        public string Password { get; set; } = null!;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
