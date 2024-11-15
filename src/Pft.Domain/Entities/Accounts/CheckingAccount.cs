using Pft.Domain.Shared;

namespace Pft.Domain.Entities.Accounts;

public class CheckingAccount : Account
{
    public bool HasDebitCard { get; private set; }
    public override AccountType AccountType => AccountType.Checking;

    public CheckingAccount(
        AccountId id, 
        string accountName, 
        Currency currency, 
        Money balance,
        bool hasDebitCard) 
        : base(id, accountName, currency, balance)
    {
        HasDebitCard = hasDebitCard;
    }

    private CheckingAccount() { }
} 