using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos.AccountsDto.Requests
{
    public class CreateBankAccountWithdrawal
    {
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        [Range(100, 1000000, ErrorMessage = "Amount withdrawal is between 100 - 1000000")]
        public double Amount { get; set; }
        [Required]
        [Range(1000, 9999, ErrorMessage = "Pin must be of 4 digits.")]
        public int Pin { get; set; }

    }
}
