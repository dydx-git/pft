using Pft.Domain.Shared;

namespace Pft.Domain.Entities.Accounts;

public class SavingsAccount : Account
{
    public decimal InterestRate { get; private set; }
    public decimal MinimumBalance { get; private set; }
    public DateTime LastInterestCalculationDate { get; private set; }
    public override AccountType AccountType => AccountType.Savings;

    public SavingsAccount(
        AccountId id, 
        string accountName, 
        Currency currency, 
        Money balance,
        decimal interestRate,
        decimal minimumBalance) 
        : base(id, accountName, currency, balance)
    {
        InterestRate = interestRate;
        MinimumBalance = minimumBalance;
        LastInterestCalculationDate = DateTime.UtcNow;
    }

    private SavingsAccount() { }
} 