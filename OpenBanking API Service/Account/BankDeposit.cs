namespace OpenBanking_API_Service_Common.Library.Entities.Account
{
    public class BankDeposit
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public double Amount { get; set; }

        public double Balance { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public Guid BankAccountId { get; set; }
        public virtual BankAccount BankAccount { get; set; }
    }
}
