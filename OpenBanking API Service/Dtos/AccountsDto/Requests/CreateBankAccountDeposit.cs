using System.ComponentModel.DataAnnotations;
namespace OpenBanking_API_Service.Dtos.AccountsDto.Requests
{
    public class CreateBankAccountDeposit
    {
        [Required(ErrorMessage = "Account number is required")]
        [StringLength(11, ErrorMessage = "Account number must be of 11 characters")]
        public string AccountNumber { get; set; } = null!;
        [Required(ErrorMessage = "Amount to be deposited is required")]
        [Range(100, double.MaxValue, ErrorMessage = "Minimum deposit amount is 100")]
        public double Amount { get; set; }
        [Required(ErrorMessage = "Pin is required")]
        [Range(1000, 9999, ErrorMessage = "Pin must be 4 characters")]
        public int Pin { get; set; }

    }
}