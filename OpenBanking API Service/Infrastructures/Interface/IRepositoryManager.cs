namespace OpenBanking_API_Service.Infrastructures.Interface
{
    public interface IRepositoryManager
    {
        IBankAccountRepository Account { get; }
        IBankDepositRepository BankDeposit { get; }
        IBankWithdrawalRepository BankWithdrawal { get; }
        IBankTransferRepository BankTransfer { get; }

        Task SaveAsync();
    }
}
