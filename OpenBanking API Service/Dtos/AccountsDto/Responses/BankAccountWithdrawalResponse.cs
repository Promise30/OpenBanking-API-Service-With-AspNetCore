namespace OpenBanking_API_Service.Dtos.AccountsDto.Responses
{
    public class BankAccountWithdrawalResponse
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public double Amount { get; set; }
        public double AccountBalance { get; set; }
        public Guid AccountId { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}
