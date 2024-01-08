namespace OpenBanking_API_Service.Infrastructures.Interface
{
    public interface IRepositoryManager
    {
        IAccountRepository Account { get; }
        void Save();
    }
}
