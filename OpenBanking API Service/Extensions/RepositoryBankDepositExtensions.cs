using OpenBanking_API_Service.Domain.Entities.Account;
using System.Linq.Dynamic.Core;

namespace OpenBanking_API_Service.Extensions
{
    public static class RepositoryBankDepositExtensions
    {
        public static IQueryable<BankDeposit> FilterBankDepositsByAmount(this IQueryable<BankDeposit> bankDeposits, double minAmount, double maxAmount) =>
            bankDeposits.Where(d => (d.Amount >= minAmount && d.Amount <= maxAmount));
        public static IQueryable<BankDeposit> FilterBankDepositsByDate(this IQueryable<BankDeposit> bankDeposits, DateTimeOffset startDate, DateTimeOffset endDate) =>
            bankDeposits.Where(d => d.TransactionDate >= startDate && d.TransactionDate <= endDate);
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
