using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos.AccountsDto.Requests
{
    public class CreateBankAccountWithdrawal
    {
        [Required(ErrorMessage = "Account number is required")]
        [StringLength(11, ErrorMessage = "Account number must be of 11 characters")]
        public string AccountNumber { get; set; } = null!;

        [Required(ErrorMessage = "Amount to be withdrawn is required")]
        [Range(100, 1000000, ErrorMessage = "Amount to be withdrawn must be between 100 - 1000000")]
        public double Amount { get; set; }
        [Required(ErrorMessage = "Pin is required")]
        [Range(1000, 9999, ErrorMessage = "Pin must be of 4 digits.")]
        public int Pin { get; set; }

    }
}
