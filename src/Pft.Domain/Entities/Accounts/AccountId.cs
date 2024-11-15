namespace Pft.Domain.Entities.Accounts;

public record AccountId(Guid Value)
{
    public static AccountId New() => new(Guid.NewGuid());
}