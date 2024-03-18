using Microsoft.EntityFrameworkCore;
using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.Extensions;
using OpenBanking_API_Service.Infrastructures.Interface;
using OpenBanking_API_Service.RequestFeatures;

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
        public async Task<PagedList<BankWithdrawal>> GetBankAccountWithdrawalsAsync(Guid accountId, AccountTransactionParameters accountTransactionParameters, bool trackChanges)
        {
            var withdrawals = await FindByCondition(d => d.AccountId.Equals(accountId) && (d.Amount >= accountTransactionParameters.MinAmount && d.Amount <= accountTransactionParameters.MaxAmount), trackChanges)
            .FilterBankWithdrawals(accountTransactionParameters.MinAmount, accountTransactionParameters.MaxAmount)
            .Sort(accountTransactionParameters.OrderBy)
            .ToListAsync();

            return PagedList<BankWithdrawal>.ToPagedList(withdrawals, accountTransactionParameters.PageNumber, accountTransactionParameters.PageSize);
        }

    }
}
