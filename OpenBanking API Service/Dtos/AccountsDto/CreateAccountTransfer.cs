namespace OpenBanking_API_Service.Dtos.AccountsDto
{
    public class CreateAccountTransfer
    {
        public string SourceAccount { get; set; }
        public double Amount { get; set; }
        public string Narration { get; set; }
        public string DestinationAccount { get; set; }
        public int Pin { get; set; }
    }
}
