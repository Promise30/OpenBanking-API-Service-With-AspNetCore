namespace OpenBanking_API_Service.Entities.Account
{
    public class BankDeposit
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public double Amount { get; set; }

        public double Balance { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public Guid AccountId { get; set; }
        public BankAccount BankAccount { get; set; }
    }
}
