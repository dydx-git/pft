using Pft.Domain.Entities.Transactions;

namespace Pft.Domain.Entities.Accounts;

public interface IHasTransactions
{
    IReadOnlyList<Transaction> Transactions { get; }
}