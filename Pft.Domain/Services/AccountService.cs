using Pft.Domain.Entities;
using Pft.Domain.ValueObjects;

namespace Pft.Domain.Services;

public class AccountService
{
    public void TransferMoney(Account fromAccount, Account toAccount, Money amount, string description)
    {
        if (fromAccount.Currency != toAccount.Currency)
            throw new InvalidOperationException("Currency mismatch during transfer");

        var transactionFrom = new Transaction(Guid.NewGuid(), fromAccount, description, amount, TransactionType.Transfer, DateTime.UtcNow, null, new List<string>(), null, null);
        var transactionTo = new Transaction(Guid.NewGuid(), toAccount, description, amount, TransactionType.Income, DateTime.UtcNow, null, new List<string>(), null, null);

        fromAccount.ApplyTransaction(transactionFrom);
        toAccount.ApplyTransaction(transactionTo);
    }
}
