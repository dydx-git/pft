using Pft.Domain.ValueObjects;

namespace Pft.Domain.Entities;

public record CreditCardAccount(Guid AccountId, string AccountName, Currency Currency, Money CreditLimit) : Account(AccountId, AccountName, Currency)
{
    public bool CanMakePurchase(Money amount) => Balance + CreditLimit >= amount;
}