using OpenBanking_API_Service.Domain.Entities.Account;
using System.Linq.Dynamic.Core;

namespace OpenBanking_API_Service.Extensions
{
    public static class RepositoryBankTransferExtensions
    {
        public static object BindingFlags { get; private set; }

        public static IQueryable<BankTransfer> FilterBankTransfers(this IQueryable<BankTransfer> bankTransfers, uint minAmount, uint maxAmount) =>
            bankTransfers.Where(d => (d.Amount >= minAmount && d.Amount <= maxAmount));

        public static IQueryable<BankTransfer> Search(this IQueryable<BankTransfer> bankTransfers, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return bankTransfers;
            }
            var lowerCase = searchTerm.ToLower();
            //return bankDeposits.Where(e => e.Amount.ToLower().Contains(lowerCase));
            return null;
        }
        public static IQueryable<BankTransfer> Sort(this IQueryable<BankTransfer> bankTransfers, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return bankTransfers.OrderBy(d => d.TransactionDate);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<BankTransfer>(orderByQueryString);
            if (string.IsNullOrEmpty(orderQuery))
                return bankTransfers.OrderBy(d => d.TransactionDate);
            return bankTransfers.OrderBy(orderQuery);
        }
    }
}
