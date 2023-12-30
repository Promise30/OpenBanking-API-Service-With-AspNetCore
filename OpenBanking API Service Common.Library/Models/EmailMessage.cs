using MimeKit;

namespace OpenBanking_API_Service_Common.Library.Models
{
    public class EmailMessage
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailMessage(IEnumerable<string> to, string subject, string body)
        {

            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("email", x)));
            Subject = subject;
            Body = body;
        }
    }
}
