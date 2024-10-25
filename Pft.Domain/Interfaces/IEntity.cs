using Pft.Domain.Entities;
using Pft.Domain.ValueObjects;

namespace Pft.Domain.Interfaces;

public interface IEntity
{
    Guid Id { get; }
}

public interface IHasBalance
{
    Money Balance { get; }
}

public interface IHasTransactions
{
    IReadOnlyList<Transaction> Transactions { get; }
}