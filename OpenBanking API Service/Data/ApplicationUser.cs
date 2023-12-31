
using Microsoft.AspNetCore.Identity;
using OpenBanking_API_Service_Common.Library.Entities.Account;


namespace OpenBanking_API_Service.Data
{
    public class ApplicationUser : IdentityUser
    {
        public DateTimeOffset CreatedAt { get; set; }
        public virtual BankAccount BankAccount { get; set; }
    }
}
