using OpenBanking_API_Service.Domain.Entities.Account;
using System.Linq.Dynamic.Core;

namespace OpenBanking_API_Service.Extensions
{
    public static class RepositoryBankAccountExtensions
    {
        public static IQueryable<BankAccount> FilterBankAccounts(this IQueryable<BankAccount> accounts, double minAmount, double maxAmount) => accounts.Where(a => (a.AccountBalance >= minAmount && a.AccountBalance <= maxAmount));
        public static IQueryable<BankAccount> Search(this IQueryable<BankAccount> accounts, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return accounts;
            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return accounts.Where(a => a.FirstName.ToLower().Contains(lowerCaseTerm)
                                || a.LastName.ToLower().Contains(lowerCaseTerm)
                                || a.MiddleName.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<BankAccount> Sort(this IQueryable<BankAccount> accounts, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return accounts.OrderBy(e => e.FirstName);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<BankAccount>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderQuery))
                return accounts.OrderBy(e => e.FirstName);
            return accounts.OrderBy(orderQuery);
        }

    }

}
