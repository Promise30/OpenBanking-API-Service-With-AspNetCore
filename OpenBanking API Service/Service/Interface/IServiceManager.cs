namespace OpenBanking_API_Service.Service.Interface
{
    public interface IServiceManager
    {
        IEmailService EmailService { get; }
        IBankAccountService BankAccountService { get; }

        ITransactionService TransactionService { get; }
    }
}
