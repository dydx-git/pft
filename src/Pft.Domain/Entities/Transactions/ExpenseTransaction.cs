using Pft.Domain.Entities.Accounts;
using Pft.Domain.Shared;

namespace Pft.Domain.Entities.Transactions;

public class ExpenseTransaction : Transaction
{
    public ExpenseCategory Category { get; }

    public ExpenseTransaction(
        TransactionId id,
        DateTime date,
        Money amount,
        AccountId accountId,
        RecurringTransaction? recurrence,
        ExpenseCategory category)
        : base(id, date, amount, accountId, recurrence)
    {
        Category = category;
    }
    
    private ExpenseTransaction() { }

    public override void ApplyToAccount(Account account)
    {
        account.DecreaseBalance(Amount);
    }
}