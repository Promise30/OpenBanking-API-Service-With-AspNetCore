namespace OpenBanking_API_Service.Dtos.AccountsDto
{
    public class CreateAccountTransferResponse
    {
        public string SourceAccount { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public string Narration { get; set; }
        public string DestinationAccount { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}
