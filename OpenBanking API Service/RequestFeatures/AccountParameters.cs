namespace OpenBanking_API_Service.RequestFeatures
{
    public class AccountParameters : RequestParameters
    {
        public AccountParameters() => OrderBy = "firstName";
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; } = double.MaxValue;

        public bool ValidAmountRange => MaxAmount > MinAmount;

        public string? SearchTerm { get; set; }


    }
}
