namespace OpenBanking_API_Service_Common.Library.Entities.Account
{
    public class BankWithdrawal
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public string AccountNumber { get; set; }
        public Guid AccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}
