using Pft.Domain.Entities.Accounts;
using Pft.Domain.Shared;

namespace Pft.Domain.Entities.Transactions;

public class TransferTransaction : Transaction
{
    public AccountId DestinationAccountId { get; }
    public Money TransferFee { get; }

    public TransferTransaction(
        TransactionId id,
        DateTime date,
        Money amount,
        AccountId sourceAccountId,
        AccountId destinationAccountId,
        RecurringTransaction? recurrence,
        Money transferFee)
        : base(id, date, amount, sourceAccountId, recurrence)
    {
        DestinationAccountId = destinationAccountId;
        TransferFee = transferFee;
    }
    
    private TransferTransaction() { }

    public override void ApplyToAccount(Account account)
    {
        if (account.Id == AccountId) // Source Account
        {
            account.DecreaseBalance(Amount + TransferFee);
        }
        else if (account.Id == DestinationAccountId) // Destination Account
        {
            account.IncreaseBalance(Amount);
        }
    }
}