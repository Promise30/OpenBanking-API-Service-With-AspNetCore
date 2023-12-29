using OpenBanking_API_Service_Common.Library.Models;

namespace OpenBanking_API_Service.Service.Interface
{
    public interface IEmailService
    {
        string SendEmail(EmailMessage message);
    }
}
