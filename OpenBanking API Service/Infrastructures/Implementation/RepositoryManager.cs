using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Infrastructures.Interface;

namespace OpenBanking_API_Service.Infrastructures.Implementation
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly Lazy<IBankAccountRepository> _accountRepository;
        private readonly Lazy<IBankDepositRepository> _bankDepositRepository;
        private readonly Lazy<IBankWithdrawalRepository> _bankWithdrawalRepository;
        private readonly Lazy<IBankTransferRepository> _bankTransferRepository;

        public RepositoryManager(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _accountRepository = new Lazy<IBankAccountRepository>(() => new BankAccountRepository(applicationDbContext));
            _bankDepositRepository = new Lazy<IBankDepositRepository>(() => new BankDepositRepository(applicationDbContext));
            _bankWithdrawalRepository = new Lazy<IBankWithdrawalRepository>(() => new BankWithdrawalRepository(applicationDbContext));
            _bankTransferRepository = new Lazy<IBankTransferRepository>(() => new BankTransferRepository(applicationDbContext));


        }
        public IBankAccountRepository Account => _accountRepository.Value;

        public IBankDepositRepository BankDeposit => _bankDepositRepository.Value;

        public IBankWithdrawalRepository BankWithdrawal => _bankWithdrawalRepository.Value;

        public IBankTransferRepository BankTransfer => _bankTransferRepository.Value;

        public async Task SaveAsync() =>
            await _applicationDbContext.SaveChangesAsync();

    }
}
