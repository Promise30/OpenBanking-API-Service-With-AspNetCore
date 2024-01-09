namespace OpenBanking_API_Service.Dtos.AccountsDto.Responses
{
    public class BankAccountWithdrawalResponse
    {
        public string AccountNumber { get; set; }
        public double Amount { get; set; }
        public double AccountBalance { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}
