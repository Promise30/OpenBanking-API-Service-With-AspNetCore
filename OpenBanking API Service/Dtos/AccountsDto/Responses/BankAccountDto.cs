namespace OpenBanking_API_Service.Dtos.AccountsDto.Responses
{
    public class BankAccountDto
    {
        public Guid BankAccountId { get; set; }
        public string AccountNumber { get; set; }
        public double AccountBalance { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ResidentCountry { get; set; }
        public string ResidentAddress { get; set; }
        public string? ResidentPostalCode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string BirthCountry { get; set; }
        public string MaritalStatus { get; set; }
        public string AccountType { get; set; }

        // Account Holder Information
        public int Pin { get; set; }
        public string UserId { get; set; }

        public DateTimeOffset AccountOpeningDate { get; set; }

    }
}
