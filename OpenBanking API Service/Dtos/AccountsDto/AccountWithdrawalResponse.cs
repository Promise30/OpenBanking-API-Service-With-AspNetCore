namespace OpenBanking_API_Service.Dtos.AccountsDto
{
    public class AccountWithdrawalResponse
    {
        public string AccountNumber { get; set; }
        public double DebitAmount { get; set; }
        public double Balance { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}
