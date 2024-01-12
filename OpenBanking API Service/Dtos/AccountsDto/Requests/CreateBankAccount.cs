using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos.AccountsDto.Requests
{
    public class CreateBankAccount
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string ResidentCountry { get; set; }
        [Required]
        public string ResidentAddress { get; set; }
        public string? ResidentPostalCode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        [Required]
        public string BirthCountry { get; set; }
        [Required]
        public string MaritalStatus { get; set; }
        [Required]
        [Range(1000, 9999, ErrorMessage = "Pin must be of 6 digits.")]
        public int Pin { get; set; }

    }
}
