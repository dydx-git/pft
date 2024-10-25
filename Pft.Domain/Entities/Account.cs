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

public record CheckingAccount(Guid AccountId, string AccountName, Currency Currency) : Account(AccountId, AccountName, Currency)
{
}

public record SavingsAccount(Guid AccountId, string AccountName, Currency Currency) : Account(AccountId, AccountName, Currency)
{
}

public record CreditCardAccount(Guid AccountId, string AccountName, Currency Currency, Money CreditLimit) : Account(AccountId, AccountName, Currency)
{
    public bool CanMakePurchase(Money amount) => Balance + CreditLimit >= amount;
}

public record Transaction(Guid Id, Account RelatedAccount, string Description, Money Amount, TransactionType Type, DateTime Date, string? Category, List<string> Tags, RecurringTransaction? Recurrence, Money? TransferFee) : IEntity
{
    public bool IsRecurring() => Recurrence is not null;
    public override string ToString() => $"{Description} ({Type}) - {Amount}";
}

public record Budget(Guid Id, string Category, Money Limit, BudgetPeriod Period) : IEntity, IHasTransactions
{
    private readonly List<Transaction> _transactions = [];
    public Money Spent => _transactions.Where(t => t.Type == TransactionType.Expense).Aggregate(Money.Zero(Limit.Currency), (total, t) => total + t.Amount);
    public IReadOnlyList<Transaction> Transactions => _transactions;
    public Money Remaining => Limit - Spent;
    public bool IsOverBudget() => Spent.Amount > Limit.Amount;
    public void AddExpense(Transaction expense)
    {
        if (expense.Amount.Currency != Limit.Currency)
            throw new InvalidOperationException("Currency mismatch for budget");

        _transactions.Add(expense);
    }
    public override string ToString() => $"Budget for {Category}: {Spent}/{Limit}";
}