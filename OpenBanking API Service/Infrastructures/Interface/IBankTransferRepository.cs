using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.RequestFeatures;

namespace OpenBanking_API_Service.Infrastructures.Interface
{
    public interface IBankTransferRepository
    {
        Task<PagedList<BankTransfer>> GetBankAccountTransfersAsync(Guid accountId, AccountTransactionParameters accountTransactionParameters, bool trackChanges);
        Task<BankTransfer> GetBankAccountTransferAsync(Guid accountId, Guid id, bool trackChanges);

        void CreateBankTransfer(BankTransfer bankTransfer);
    }
}
