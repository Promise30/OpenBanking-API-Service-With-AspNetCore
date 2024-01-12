using OpenBanking_API_Service.Domain.Entities.Account;

namespace OpenBanking_API_Service.Infrastructures.Interface
{
    public interface IBankDepositRepository
    {
        /// <summary>
        /// These method definitions reflect the GET requests
        /// </summary>
        Task<IEnumerable<BankDeposit>> GetBankAccountDepositsAsync(Guid accountId, bool trackChanges);
        Task<BankDeposit> GetBankAccountDepositAsync(Guid accountId, Guid id, bool trackChanges);

        void CreateBankDeposit(BankDeposit bankDeposit);

    }
}
