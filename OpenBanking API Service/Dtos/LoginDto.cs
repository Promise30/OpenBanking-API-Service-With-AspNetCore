using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email field is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password field is required")]
        [MinLength(8, ErrorMessage = "Password cannot be less than 8 characters")]
        public string Password { get; set; }
    }
}
