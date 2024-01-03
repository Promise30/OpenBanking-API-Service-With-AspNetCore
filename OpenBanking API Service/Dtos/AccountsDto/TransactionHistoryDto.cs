namespace OpenBanking_API_Service.Dtos.AccountsDto
{
    public class TransactionHistoryDto
    {
        public IEnumerable<CreateBankDepositResponse> BankDeposits { get; set; }
        public IEnumerable<AccountWithdrawalResponse> BankWithdrawals { get; set; }
        public IEnumerable<CreateAccountTransferResponse> BankTransfers { get; set; }
    }
}
