
using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.RequestFeatures;

namespace OpenBanking_API_Service.Infrastructures.Interface;

public interface IBankAccountRepository
{
    Task<PagedList<BankAccount>> GetAllAccountsAsync(AccountParameters accountParameters, bool trackChanges);
    Task<BankAccount> GetBankAccountAsync(Guid accountId, bool trackChanges);
    bool AccountExists(string accountNumber, bool trackChanges);
    void CreateBankAccount(BankAccount bankAccount);
    Task<BankAccount> GetBankAccountByAccountNumberAsync(string accountNumber, bool trackChanges);
    void DeleteBankAccount(BankAccount bankAccount);

}
