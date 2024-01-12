using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos.AccountsDto.Requests
{
    public class CreateBankAccountTransfer
    {
        [Required]
        public string SourceAccount { get; set; }
        [Required]
        [Range(100, 1000000, ErrorMessage = "Daily transaction limit cannot exceed 1_000_000")]
        public double Amount { get; set; }
        [MaxLength(20, ErrorMessage ="Maximum length is 20 characters")]
        public string? Narration { get; set; }
        [Required]
        public string DestinationAccount { get; set; }
        [Required]
        [Range(1000, 9999, ErrorMessage = "Pin must be of 4 digits.")]
        public int Pin { get; set; }
    }
}
