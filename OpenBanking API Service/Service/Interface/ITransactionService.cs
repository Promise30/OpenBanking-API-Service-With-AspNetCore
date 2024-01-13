using OpenBanking_API_Service.Dtos.AccountsDto.Requests;
using OpenBanking_API_Service.Dtos.AccountsDto.Responses;
using OpenBanking_API_Service.RequestFeatures;

namespace OpenBanking_API_Service.Service.Interface
{
    public interface ITransactionService
    {
        /// <summary>
        /// Method Definitions for Bank Deposits
        /// </summary>

        Task<(APIResponse<IEnumerable<BankAccountDepositResponse>>, MetaData metaData)> GetBankAccountDepositsAsync(Guid accountId, AccountTransactionParameters accountTransactionParameters, bool trackChanges);
        Task<APIResponse<BankAccountDepositResponse>> GetBankAccountDepositAsync(Guid accountId, Guid id, bool trackChanges);

        Task<APIResponse<BankAccountDepositResponse>> CreateBankAccountDepositAsync(CreateBankAccountDeposit createBankAccountDeposit, bool trackChanges);

        ///<summary>
        /// Method defintions for Bank Withdrawals
        ///</summary>
        ///
        Task<(APIResponse<IEnumerable<BankAccountWithdrawalResponse>>, MetaData metaData)> GetBankAccountWithdrawalsAsync(Guid accountId, AccountTransactionParameters accountTransactionParameters, bool trackChanges);
        Task<APIResponse<BankAccountWithdrawalResponse>> GetBankAccountWithdrawalAsync(Guid accountId, Guid id, bool trackChanges);
        Task<APIResponse<BankAccountWithdrawalResponse>> CreateBankAccountWithdrawalAsync(CreateBankAccountWithdrawal createBankAccountWithdrawal, bool trackChanges);


        ///<summary>
        /// Method definitions for Bank Transfers
        ///</summary>

        Task<(APIResponse<IEnumerable<BankAccountTransferResponse>>, MetaData metaData)> GetBankAccountTransfersAsync(Guid accountId, AccountTransactionParameters accountTransactionParameters, bool trackChanges);
        Task<APIResponse<BankAccountTransferResponse>> GetBankAccountTransferAsync(Guid accountId, Guid id, bool trackChanges);
        Task<APIResponse<BankAccountTransferResponse>> CreateBankAccountTransferAsync(CreateBankAccountTransfer createBankAccountTransfer, bool trackChanges);

    }
}
