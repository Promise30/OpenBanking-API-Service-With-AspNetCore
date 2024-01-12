using OpenBanking_API_Service.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenBanking_API_Service.Domain.Entities.Account
{
    public class BankAccount
    {
        public Guid BankAccountId { get; set; }
        public string AccountNumber { get; set; }
        public double AccountBalance { get; set; } = 0.0D;
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

        // Account Holder Information
        public int Pin { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTimeOffset AccountOpeningDate { get; set; } = DateTimeOffset.Now;
        public ICollection<BankTransfer> BankTransfers { get; } = new List<BankTransfer>();
        public ICollection<BankDeposit> BankDeposits { get; set; }
        public ICollection<BankWithdrawal> BankWithdrawals { get; } = new List<BankWithdrawal>();
    }
}
