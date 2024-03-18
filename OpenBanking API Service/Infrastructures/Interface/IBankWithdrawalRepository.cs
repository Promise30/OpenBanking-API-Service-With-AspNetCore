using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.RequestFeatures;

namespace OpenBanking_API_Service.Infrastructures.Interface
{
    public interface IBankWithdrawalRepository
    {
        Task<PagedList<BankWithdrawal>> GetBankAccountWithdrawalsAsync(Guid accountId, AccountTransactionParameters accountTransactionParameters, bool trackChanges);
        Task<BankWithdrawal> GetBankAccountWithdrawalAsync(Guid accountId, Guid id, bool trackChanges);

        void CreateBankWithdrawal(BankWithdrawal bankWithdrawal);
    }
}
