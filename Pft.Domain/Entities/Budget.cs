using Pft.Domain.Interfaces;
using Pft.Domain.ValueObjects;

namespace Pft.Domain.Entities;

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