
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenBanking_API_Service.Domain.Entities.Account;

namespace OpenBanking_API_Service.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankTransfer> BankTransfers { get; set; }
        public DbSet<BankDeposit> BankDeposits { get; set; }
        public DbSet<BankWithdrawal> BankWithdrawals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.BankAccount)
                .WithOne(a => a.User)
                .HasForeignKey<BankAccount>(b => b.UserId)
                .IsRequired();

            builder.Entity<BankAccount>()
                .HasMany(b => b.BankTransfers)
                .WithOne(a => a.BankAccount)
                .HasForeignKey(a => a.AccountId)
                .IsRequired();

            builder.Entity<BankAccount>()
                .HasMany(b => b.BankDeposits)
                .WithOne(a => a.BankAccount)
                .HasForeignKey(a => a.AccountId)
                .IsRequired();

            builder.Entity<BankAccount>()
                .HasMany(b => b.BankWithdrawals)
                .WithOne(a => a.BankAccount)
                .HasForeignKey(a => a.AccountId)
                .IsRequired();
        }



        #region Private methods
        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData
                (
                new IdentityRole { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", ConcurrencyStamp = "2", NormalizedName = "USER" }
                );
        }

        #endregion
    }
}
