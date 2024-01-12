using System.ComponentModel.DataAnnotations;
namespace OpenBanking_API_Service.Dtos.AccountsDto.Requests
{
    public class CreateBankAccountDeposit
    {
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        [Range(100, double.MaxValue, ErrorMessage = "Minimum deposit amount is 100")]
        public double Amount { get; set; }
        [Required]
        [Range(1000, 9999, ErrorMessage = "Pin cannot be more than 4 digits")]
        public int Pin { get; set; }

    }
}