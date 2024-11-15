using Pft.Domain.Entities.Transactions;
using Pft.Domain.Shared;

namespace Pft.Domain.Entities.Accounts;

public abstract class Account : Entity<AccountId>, IHasBalance, IHasTransactions
{
    private readonly List<Transaction> _transactions = [];
    public string AccountName { get; }
    public Currency Currency { get; }
    public Money Balance { get; private set; }
    public IReadOnlyList<Transaction> Transactions => _transactions;
    public abstract AccountType AccountType { get; }

    protected Account(AccountId id, string accountName, Currency currency, Money balance) 
        : base(id)
    {
        Id = id;
        AccountName = accountName;
        Currency = currency;
        Balance = balance;
    }
    
    protected Account() { }

    public void AddTransaction(Transaction transaction)
    {
        _transactions.Add(transaction);
        transaction.ApplyToAccount(this);
    }

    public void IncreaseBalance(Money amount)
    {
        Balance += amount;
    }

    public void DecreaseBalance(Money amount)
    {
        Balance -= amount;
    }
}