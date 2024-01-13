using OpenBanking_API_Service.Domain.Entities.Account;
using System.Linq.Dynamic.Core;

namespace OpenBanking_API_Service.Extensions
{
    public static class RepositoryBankDepositExtensions
    {
        public static object BindingFlags { get; private set; }

        public static IQueryable<BankDeposit> FilterBankDeposits(this IQueryable<BankDeposit> bankDeposits, uint minAmount, uint maxAmount) =>
            bankDeposits.Where(d => (d.Amount >= minAmount && d.Amount <= maxAmount));

        public static IQueryable<BankDeposit> Search(this IQueryable<BankDeposit> bankDeposits, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return bankDeposits;
            }
            var lowerCase = searchTerm.ToLower();
            //return bankDeposits.Where(e => e.Amount.ToLower().Contains(lowerCase));
            return null;
        }
        public static IQueryable<BankDeposit> Sort(this IQueryable<BankDeposit> bankDeposits, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return bankDeposits.OrderBy(d => d.TransactionDate);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<BankDeposit>(orderByQueryString);
            if (string.IsNullOrEmpty(orderQuery))
                return bankDeposits.OrderBy(d => d.TransactionDate);
            return bankDeposits.OrderBy(orderQuery);
        }
    }
}
