using OpenBanking_API_Service.Data;

namespace OpenBanking_API_Service_Common.Library.Entities.Account
{
    public class BankAccount
    {
        public Guid BankAccountId { get; set; }
        public string AccountNumber { get; set; }
        public decimal AccountBalance { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string StateOfOrigin { get; set; }
        public string MaritalStatus { get; set; }

        // Account Holder Information
        public int Pin { get; set; }
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTimeOffset AccountOpeningDate { get; set; }
        public ICollection<BankTransfer> BankTransfers { get; } = new List<BankTransfer>();
        public ICollection<BankDeposit> BankDeposits { get; } = new List<BankDeposit>();
        public ICollection<BankWithdrawal> BankWithdrawals { get; } = new List<BankWithdrawal>();
    }
}
