using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos.AuthenticationDtos.Requests
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Username field is required")]
        public string UserName { get; init; } = string.Empty;

        public string? PhoneNumber { get; init; }

        [EmailAddress]
        [Required(ErrorMessage = "Email field is required")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password field is required")]
        [MinLength(8, ErrorMessage = "Password cannot be less than 8 characters")]
        public string Password { get; set; } = null!;

        //public ICollection<string>? Roles { get; init; }
        public bool EnableTwoFactor { get; init; }
    }
}
