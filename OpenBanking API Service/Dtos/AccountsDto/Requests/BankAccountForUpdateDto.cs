using OpenBanking_API_Service.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace OpenBanking_API_Service.Dtos.AccountsDto.Requests
{
    public class BankAccountForUpdateDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; } = string.Empty;

        public string? MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Resident country is required")]
        public string ResidentCountry { get; set; } = string.Empty;

        [Required(ErrorMessage = "Resident address is required.")]
        public string ResidentAddress { get; set; } = string.Empty;

        [StringLength(6, ErrorMessage = "Postal code cannot be more than 6 characters.")]
        public string? ResidentPostalCode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        [Required(ErrorMessage = "Birth country is required.")]
        public string BirthCountry { get; set; } = string.Empty;
        [Required(ErrorMessage = "Marital status is required")]
        public MaritalStatus MaritalStatus { get; set; }
        [Required(ErrorMessage = "Account type is required")]
        public AccountType AccountType { get; set; }

        [Required]
        [Range(1000, 9999, ErrorMessage = "Pin must be 4 characters.")]
        public int Pin { get; set; }
    }
}
