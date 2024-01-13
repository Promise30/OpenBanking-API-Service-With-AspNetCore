using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.RequestFeatures;

namespace OpenBanking_API_Service.Infrastructures.Interface
{
    public interface IBankDepositRepository
    {
        /// <summary>
        /// These method definitions reflect the GET requests
        /// </summary>
        Task<PagedList<BankDeposit>> GetBankAccountDepositsAsync(Guid accountId, AccountTransactionParameters accountTransactionParameters, bool trackChanges);
        Task<BankDeposit> GetBankAccountDepositAsync(Guid accountId, Guid id, bool trackChanges);

        void CreateBankDeposit(BankDeposit bankDeposit);

    }
}
