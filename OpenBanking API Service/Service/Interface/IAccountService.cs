using OpenBanking_API_Service.Dtos.AccountsDto;

namespace OpenBanking_API_Service.Service.Interface
{
    public interface IAccountService
    {
        Task<APIResponse<CreateAccountResponse>> CreateBankAccount(CreateBankAccountDto createBankAccountDto);
        Task<APIResponse<CreateBankDepositResponse>> BankAccountDeposit(string accountNumber, CreateBankAccountDepositDto bankAccountDepositDto);
        Task<APIResponse<AccountWithdrawalResponse>> BankAccountWithdrawal(CreateBankAccountWithdrawal bankAccountWithdrawal);
        Task<APIResponse<CreateAccountTransferResponse>> BankAccountTransfer(CreateAccountTransfer bankAccountTransfer);
        Task<APIResponse<TransactionHistoryDto>> BankAccountTransactionHistory(Guid accountId);
    }
}
