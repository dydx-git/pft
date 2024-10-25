using Pft.Domain.Entities;

namespace Pft.Domain.Interfaces;

public interface IHasTransactions
{
    IReadOnlyList<Transaction> Transactions { get; }
}