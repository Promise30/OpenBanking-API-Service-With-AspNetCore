﻿using Microsoft.EntityFrameworkCore;
using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.Extensions;
using OpenBanking_API_Service.Infrastructures.Interface;
using OpenBanking_API_Service.RequestFeatures;

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

        public async Task<PagedList<BankDeposit>> GetBankAccountDepositsAsync(Guid accountId, AccountTransactionParameters accountTransactionParameters, bool trackChanges)
        {
            var deposits = await FindByCondition(d => d.AccountId.Equals(accountId), trackChanges)
                            .FilterBankDepositsByAmount(accountTransactionParameters.MinAmount, accountTransactionParameters.MaxAmount)
                            .FilterBankDepositsByDate(accountTransactionParameters.StartDate, accountTransactionParameters.EndDate)
                            .Sort(accountTransactionParameters.OrderBy)
                            .ToListAsync();

            return PagedList<BankDeposit>.ToPagedList(deposits, accountTransactionParameters.PageNumber, accountTransactionParameters.PageSize);

        }
    }
}
