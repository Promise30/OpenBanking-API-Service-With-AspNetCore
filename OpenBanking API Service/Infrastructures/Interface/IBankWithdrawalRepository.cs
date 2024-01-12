using OpenBanking_API_Service.Domain.Entities.Account;

namespace OpenBanking_API_Service.Infrastructures.Interface
{
    public interface IBankWithdrawalRepository
    {
        Task<IEnumerable<BankWithdrawal>> GetBankAccountWithdrawalsAsync(Guid accountId, bool trackChanges);
        Task<BankWithdrawal> GetBankAccountWithdrawalAsync(Guid accountId, Guid id, bool trackChanges);

        void CreateBankWithdrawal(BankWithdrawal bankWithdrawal);
    }
}
