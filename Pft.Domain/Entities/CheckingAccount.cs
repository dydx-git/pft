using Pft.Domain.ValueObjects;

namespace Pft.Domain.Entities;

public record CheckingAccount(Guid AccountId, string AccountName, Currency Currency) : Account(AccountId, AccountName, Currency)
{
}