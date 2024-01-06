using OpenBanking_API_Service.Service.Constants;

namespace OpenBanking_API_Service.Service.Interface
{
    public interface IEmailService
    {
        string SendEmail(EmailMessage message);
    }
}
