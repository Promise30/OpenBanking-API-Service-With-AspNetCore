
using Microsoft.AspNetCore.Identity;
using OpenBanking_API_Service.Domain.Entities.Account;

namespace OpenBanking_API_Service.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset ModifiedDate { get; set; } = DateTimeOffset.UtcNow;
        public virtual BankAccount BankAccount { get; set; }
    }
}
