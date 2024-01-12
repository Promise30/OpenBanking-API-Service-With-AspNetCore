using Microsoft.EntityFrameworkCore;
using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.Infrastructures.Interface;

namespace OpenBanking_API_Service.Infrastructures.Implementation
{
    public class BankWithdrawalRepository : RepositoryBase<BankWithdrawal>, IBankWithdrawalRepository
    {
        public BankWithdrawalRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public void CreateBankWithdrawal(BankWithdrawal bankWithdrawal)
        {
            Create(bankWithdrawal);
        }

        public async Task<BankWithdrawal> GetBankAccountWithdrawalAsync(Guid accountId, Guid id, bool trackChanges) =>
            await FindByCondition(d => d.AccountId.Equals(accountId) && d.Id == id, trackChanges)
            .SingleOrDefaultAsync();
        public async Task<IEnumerable<BankWithdrawal>> GetBankAccountWithdrawalsAsync(Guid accountId, bool trackChanges) =>
            await FindByCondition(d => d.AccountId.Equals(accountId), trackChanges)
            .OrderBy(d => d.TransactionDate)
            .ToListAsync();

    }
}
