using OpenBanking_API_Service.Domain.Entities.Account;
using System.Linq.Dynamic.Core;

namespace OpenBanking_API_Service.Extensions
{
    public static class RepositoryBankWithdrawalExtensions
    {
        public static object BindingFlags { get; private set; }

        public static IQueryable<BankWithdrawal> FilterBankWithdrawals(this IQueryable<BankWithdrawal> bankWithdrawals, uint minAmount, uint maxAmount) =>
            bankWithdrawals.Where(d => (d.Amount >= minAmount && d.Amount <= maxAmount));

        public static IQueryable<BankWithdrawal> Search(this IQueryable<BankWithdrawal> bankWithdrawals, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return bankWithdrawals;
            }
            var lowerCase = searchTerm.ToLower();
            //return bankDeposits.Where(e => e.Amount.ToLower().Contains(lowerCase));
            return null;
        }
        public static IQueryable<BankWithdrawal> Sort(this IQueryable<BankWithdrawal> bankWithdrawals, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return bankWithdrawals.OrderBy(d => d.TransactionDate);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<BankWithdrawal>(orderByQueryString);
            if (string.IsNullOrEmpty(orderQuery))
                return bankWithdrawals.OrderBy(d => d.TransactionDate);
            return bankWithdrawals.OrderBy(orderQuery);
        }
    }
}
