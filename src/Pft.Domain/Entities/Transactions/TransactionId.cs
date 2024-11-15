namespace Pft.Domain.Entities.Transactions;

public record TransactionId(Guid Value)
{
    public static TransactionId NewId() => new(Guid.NewGuid());
}