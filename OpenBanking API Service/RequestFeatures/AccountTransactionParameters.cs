namespace OpenBanking_API_Service.RequestFeatures
{
    public class AccountTransactionParameters : RequestParameters
    {
        public uint MinAmount { get; set; }
        public uint MaxAmount { get; set; } = int.MaxValue;

        public bool ValidAmountRange => MaxAmount > MinAmount;
        //public string? SearchTerm { get; set; }


        public AccountTransactionParameters() => OrderBy = "transactionDate";
    }
}
