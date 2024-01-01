namespace OpenBanking_API_Service.Dtos.AccountsDto
{
    public class CreateBankDepositResponse
    {
        public string AccountNumber { get; set; }
        public double Deposit { get; set; }
        public double Balance { get; set; }
        public DateTimeOffset TransactionDate { get; set; }

    }
}
