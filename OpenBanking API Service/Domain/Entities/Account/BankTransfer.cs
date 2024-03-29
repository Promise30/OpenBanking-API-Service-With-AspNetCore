﻿namespace OpenBanking_API_Service.Domain.Entities.Account
{
    public class BankTransfer
    {
        public Guid Id { get; set; }
        public string SourceAccount { get; set; }
        public double Amount { get; set; }
        public double AccountBalance { get; set; }
        public string? Narration { get; set; }
        public string DestinationAccount { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public Guid AccountId { get; set; }
        public BankAccount BankAccount { get; set; }
    }
}
