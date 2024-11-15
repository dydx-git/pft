using Pft.Domain.Entities.Accounts;
using Pft.Domain.Shared;

namespace Pft.Domain.Entities.Transactions;

public class IncomeTransaction : Transaction
{
    public IncomeCategory Category { get; }

    public IncomeTransaction(
        TransactionId id,
        DateTime date,
        Money amount,
        AccountId accountId,
        RecurringTransaction? recurrence,
        IncomeCategory category)
        : base(id, date, amount, accountId, recurrence)
    {
        Category = category;
    }
    
    private IncomeTransaction() { }

    public override void ApplyToAccount(Account account)
    {
        account.IncreaseBalance(Amount);
    }
}