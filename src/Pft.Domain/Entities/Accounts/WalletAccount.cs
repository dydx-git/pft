using Pft.Domain.Shared;

namespace Pft.Domain.Entities.Accounts;

public class WalletAccount : Account
{
    public override AccountType AccountType => AccountType.Wallet;

    public WalletAccount(
        AccountId id, 
        string accountName, 
        Currency currency, 
        Money balance) 
        : base(id, accountName, currency, balance)
    {
        
    }

    private WalletAccount() { }
}