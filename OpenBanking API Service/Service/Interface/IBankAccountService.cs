using OpenBanking_API_Service.Dtos.AccountsDto.Requests;
using OpenBanking_API_Service.Dtos.AccountsDto.Responses;
using OpenBanking_API_Service.RequestFeatures;

namespace OpenBanking_API_Service.Service.Interface
{
    public interface IBankAccountService
    {

        /// <summary>
        /// Bank Account Service GET request methods
        /// </summary>

        Task<(APIResponse<IEnumerable<BankAccountDto>>, MetaData metaData)> GetAllBankAccountsAsync(AccountParameters accountParameters, bool trackChanges);
        Task<APIResponse<BankAccountDto>> GetBankAccountAsync(Guid accountId, bool trackChanges);



        Task<APIResponse<BankAccountDto>> CreateBankAccountAsync(CreateBankAccount createBankAccountDto);
        //Task<APIResponse<BankAccountDepositResponse>> BankAccountDeposit(string accountNumber, CreateBankAccountDeposit bankAccountDepositDto);
        //Task<APIResponse<BankAccountWithdrawalResponse>> BankAccountWithdrawal(CreateBankAccountWithdrawal bankAccountWithdrawal);
        //Task<APIResponse<BankAccountTransferResponse>> BankAccountTransfer(CreateBankAccountTransfer bankAccountTransfer);
        //Task<APIResponse<TransactionHistoryDto>> BankAccountTransactionHistory(Guid accountId);
    }
}
