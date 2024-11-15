using Pft.Domain.Entities.Accounts;
using Pft.Domain.Shared;

namespace Pft.Domain.Entities.Transactions;

public abstract class Transaction : Entity<TransactionId>
{
    public DateTime Date { get; }
    public Money Amount { get; }
    public AccountId AccountId { get; }
    public List<string> Tags { get; } = [];
    public RecurringTransaction? Recurrence { get; }
    public bool IsRecurring() => Recurrence is not null;

    protected Transaction(TransactionId id,
        DateTime date,
        Money amount,
        AccountId accountId,
        RecurringTransaction? recurrence)
        : base(id)
    {
        Id = id;
        Date = date;
        Amount = amount;
        AccountId = accountId;
        Recurrence = recurrence;
    }

    protected Transaction() { }

    public abstract void ApplyToAccount(Account account);
}