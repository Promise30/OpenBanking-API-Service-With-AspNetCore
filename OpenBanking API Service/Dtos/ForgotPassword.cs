using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
