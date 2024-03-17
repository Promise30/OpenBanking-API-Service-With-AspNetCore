
using Microsoft.EntityFrameworkCore;
using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.Extensions;
using OpenBanking_API_Service.Infrastructures.Interface;
using OpenBanking_API_Service.RequestFeatures;

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

        public async Task<PagedList<BankTransfer>> GetBankAccountTransfersAsync(Guid accountId, AccountTransactionParameters accountTransactionParameters, bool trackChanges)
        {
            var transfers = await FindByCondition(d => d.AccountId.Equals(accountId) && (d.Amount >= accountTransactionParameters.MinAmount && d.Amount <= accountTransactionParameters.MaxAmount), trackChanges)
            .FilterBankTransfers(accountTransactionParameters.MinAmount, accountTransactionParameters.MaxAmount)
            .Sort(accountTransactionParameters.OrderBy)
            .ToListAsync();

            return PagedList<BankTransfer>.ToPagedList(transfers, accountTransactionParameters.PageNumber, accountTransactionParameters.PageSize);
        }



    }
}
