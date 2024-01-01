using OpenBanking_API_Service.Dtos.AccountsDto;
using OpenBanking_API_Service_Common.Library.Models;

namespace OpenBanking_API_Service.Service.Interface
{
    public interface IAccountService
    {
        Task<APIResponse<CreateAccountResponse>> CreateBankAccount(CreateBankAccountDto createBankAccountDto);
        Task<APIResponse<CreateBankDepositResponse>> BankAccountDeposit(string accountNumber, CreateBankAccountDepositDto bankAccountDepositDto);
    }
}
