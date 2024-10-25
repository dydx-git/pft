using Pft.Domain.ValueObjects;

namespace Pft.Domain.Entities;

public record SavingsAccount(Guid AccountId, string AccountName, Currency Currency) : Account(AccountId, AccountName, Currency)
{
}