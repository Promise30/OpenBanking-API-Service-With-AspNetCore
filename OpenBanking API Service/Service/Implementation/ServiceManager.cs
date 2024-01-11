using AutoMapper;
using OpenBanking_API_Service.Infrastructures.Interface;
using OpenBanking_API_Service.Service.Constants;
using OpenBanking_API_Service.Service.Interface;

namespace OpenBanking_API_Service.Service.Implementation
{
    public class ServiceManager : IServiceManager
    {

        private readonly Lazy<IBankAccountService> _bankAccountService;
        private readonly Lazy<ITransactionService> _transactionService;
        private readonly Lazy<IEmailService> _emailService;
        public ServiceManager(IRepositoryManager repositoryManager,
                                IHttpContextAccessor httpContextAccessor,
                                ILogger<BankAccountService> logger,
                                ILogger<TransactionService> transactionLogger,
                                IMapper mapper,
                                EmailConfiguration emailConfiguration

                                )
        {
            _bankAccountService = new Lazy<IBankAccountService>(() => new BankAccountService(httpContextAccessor, repositoryManager, logger, mapper));
            _transactionService = new Lazy<ITransactionService>(() => new TransactionService(repositoryManager, transactionLogger, mapper));
            _emailService = new Lazy<IEmailService>(() => new EmailService(emailConfiguration));
        }
        public IBankAccountService BankAccountService => _bankAccountService.Value;
        public ITransactionService TransactionService => _transactionService.Value;

        public IEmailService EmailService => _emailService.Value;
    }
}
