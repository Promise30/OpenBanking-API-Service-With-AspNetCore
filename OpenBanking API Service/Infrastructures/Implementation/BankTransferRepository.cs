using Microsoft.EntityFrameworkCore;
using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.Infrastructures.Interface;

namespace OpenBanking_API_Service.Infrastructures.Implementation
{
    public class BankTransferRepository : RepositoryBase<BankTransfer>, IBankTransferRepository
    {
        public BankTransferRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public void CreateBankTransfer(BankTransfer bankTransfer)
        {
            Create(bankTransfer);
        }

        public async Task<BankTransfer> GetBankAccountTransferAsync(Guid accountId, Guid id, bool trackChanges) =>
            await FindByCondition(d => d.AccountId.Equals(accountId) && d.Id == id, trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<BankTransfer>> GetBankAccountTransfersAsync(Guid accountId, bool trackChanges) =>

            await FindByCondition(d => d.AccountId.Equals(accountId), trackChanges)
            .OrderBy(d => d.TransactionDate)
            .ToListAsync();

    }
}
