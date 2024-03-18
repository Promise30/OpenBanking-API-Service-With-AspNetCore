using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.Dtos.AccountsDto.Requests;
using OpenBanking_API_Service.Dtos.AccountsDto.Responses;
using OpenBanking_API_Service.RequestFeatures;

namespace OpenBanking_API_Service.Service.Interface
{
    public interface IBankAccountService
    {
        Task<(APIResponse<IEnumerable<BankAccountDto>> accounts, MetaData metaData)> GetAllBankAccountsAsync(AccountParameters accountParameters, bool trackChanges);
        Task<APIResponse<BankAccountDto>> GetBankAccountAsync(Guid accountId, bool trackChanges);
        Task<APIResponse<BankAccountDto>> CreateBankAccountAsync(CreateBankAccount createBankAccountDto);
        Task<(BankAccountForUpdateDto bankAccountToPatch, BankAccount bankAccountEntity)> GetBankAccountForPatch(Guid accountId, bool trackChanges);
        void SaveChangesForPatch(BankAccountForUpdateDto bankAccountToPatch, BankAccount bankAccount);
        Task<APIResponse<object>> DeleteBankAccountAsync(Guid accountId, bool trackChanges);


    }
}
