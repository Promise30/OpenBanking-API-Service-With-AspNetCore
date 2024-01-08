using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Infrastructures.Interface;

namespace OpenBanking_API_Service.Infrastructures.Implementation
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly Lazy<IAccountRepository> _accountRepository;
        public RepositoryManager(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(applicationDbContext));

        }
        public IAccountRepository Account => _accountRepository.Value;

        public void Save()
        {
            _applicationDbContext.SaveChanges();
        }
    }
}
