namespace OpenBanking_API_Service.Dtos.AccountsDto.Responses
{
    public class BankAccountTransferResponse
    {
        public Guid Id { get; set; }
        public string SourceAccount { get; set; }
        public double Amount { get; set; }
        public double AccountBalance { get; set; }
        public string Narration { get; set; }
        public string DestinationAccount { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}
