using Pft.Domain.Interfaces;
using Pft.Domain.ValueObjects;

namespace Pft.Domain.Entities;

public abstract record Account(Guid Id, string AccountName, Currency Currency) : IEntity, IHasBalance, IHasTransactions
{
    private readonly List<Transaction> _transactions = [];
    public Money Balance { get; private set; } = Money.Zero(Currency);
    public IReadOnlyList<Transaction> Transactions => _transactions;

    public void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount.Currency != Currency)
            throw new InvalidOperationException("Transaction currency must match account currency");

        Balance += transaction.Type == TransactionType.Expense ? -transaction.Amount : transaction.Amount;
        _transactions.Add(transaction);
    }

}