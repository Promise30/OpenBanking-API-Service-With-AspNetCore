using OpenBanking_API_Service.Domain.Entities.Account;

namespace OpenBanking_API_Service.Infrastructures.Interface
{
    public interface IBankTransferRepository
    {
        Task<IEnumerable<BankTransfer>> GetBankAccountTransfersAsync(Guid accountId, bool trackChanges);
        Task<BankTransfer> GetBankAccountTransferAsync(Guid accountId, Guid id, bool trackChanges);

        void CreateBankTransfer(BankTransfer bankTransfer);
    }
}
