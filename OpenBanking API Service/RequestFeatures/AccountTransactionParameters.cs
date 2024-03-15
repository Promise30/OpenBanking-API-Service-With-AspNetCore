namespace OpenBanking_API_Service.RequestFeatures
{
    public class AccountTransactionParameters : RequestParameters
    {
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; } = double.MaxValue;

        public bool ValidAmountRange => MaxAmount > MinAmount;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }


        public AccountTransactionParameters() => OrderBy = "transactionDate";
    }
}
