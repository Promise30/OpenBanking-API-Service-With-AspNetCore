﻿using Microsoft.EntityFrameworkCore;
using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.Infrastructures.Interface;

namespace OpenBanking_API_Service.Infrastructures.Implementation
{
    public class BankAccountRepository : RepositoryBase<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public bool AccountExists(string accountNumber, bool trackChanges)
        {
            return FindByCondition(a => a.AccountNumber.Equals(accountNumber), trackChanges).FirstOrDefault() != null;
        }


        public void CreateBankAccount(BankAccount bankAccount)
        {
            Create(bankAccount);
        }

        public async Task<IEnumerable<BankAccount>> GetAllAccountsAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<BankAccount> GetBankAccountAsync(Guid accountId, bool trackChanges) =>

            await FindByCondition(a => a.BankAccountId.Equals(accountId), trackChanges).SingleOrDefaultAsync();


        public async Task<BankAccount> GetBankAccountByAccountNumberAsync(string accountNumber, bool trackChanges) =>

            await FindByCondition(a => a.AccountNumber.Equals(accountNumber), trackChanges).SingleOrDefaultAsync();

    }
}
