using Microsoft.EntityFrameworkCore;
using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.Infrastructures.Interface;

namespace OpenBanking_API_Service.Infrastructures.Implementation
{
    public class BankDepositRepository : RepositoryBase<BankDeposit>, IBankDepositRepository
    {
        public BankDepositRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public void CreateBankDeposit(BankDeposit bankDeposit)
        {
            Create(bankDeposit);
        }

        public async Task<BankDeposit> GetBankAccountDepositAsync(Guid accountId, Guid id, bool trackChanges) =>
           await FindByCondition(d => d.AccountId.Equals(accountId) && d.Id == id, trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<BankDeposit>> GetBankAccountDepositsAsync(Guid accountId, bool trackChanges) =>
            await FindByCondition(d => d.AccountId.Equals(accountId), trackChanges)
            .OrderBy(d => d.TransactionDate)
            .ToListAsync();
    }
}
