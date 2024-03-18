using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos.AccountsDto.Requests
{
    public class CreateBankAccountTransfer
    {
        [Required(ErrorMessage = "Source account number is required")]
        [StringLength(11, ErrorMessage = "Source account number must be of 11 characters")]
        public string SourceAccount { get; set; } = null!;

        [Required(ErrorMessage = "Amount to be transferred is required")]
        [Range(100, 1000000, ErrorMessage = "Transaction amount cannot exceed 1_000_000")]
        public double Amount { get; set; }

        [MaxLength(20, ErrorMessage = "Narration cannot exceed 30 characters")]
        public string? Narration { get; set; }

        [Required(ErrorMessage = "Destination account number is required")]
        [StringLength(11, ErrorMessage = "Destination account number must be of 11 characters")]
        public string DestinationAccount { get; set; } = null!;

        [Required(ErrorMessage = "Pin is required")]
        [Range(1000, 9999, ErrorMessage = "Pin must be of 4 digits.")]
        public int Pin { get; set; }
    }
}
