namespace Pft.Domain.ValueObjects;

public record Money(decimal Amount, Currency Currency)
{
    public static Money Zero(Currency currency) => new(0, currency);
    public bool IsZero() => Amount == 0;
    public override string ToString() => $"{Currency.Symbol} {Amount}";

    public static Money operator +(Money a, Money b) => a.Currency == b.Currency ? new Money(a.Amount + b.Amount, a.Currency) : throw new InvalidOperationException("Currency mismatch");
    public static Money operator -(Money a, Money b) => a.Currency == b.Currency ? new Money(a.Amount - b.Amount, a.Currency) : throw new InvalidOperationException("Currency mismatch");
    public static Money operator *(Money a, decimal factor) => new(a.Amount * factor, a.Currency);
    public static Money operator /(Money a, decimal divisor) => new(a.Amount / divisor, a.Currency);
    public static bool operator >=(Money a, Money b) => a.Currency == b.Currency && a.Amount >= b.Amount;
    public static bool operator <=(Money a, Money b) => a.Currency == b.Currency && a.Amount <= b.Amount;
    public static Money operator -(Money money) => new(-money.Amount, money.Currency);
}