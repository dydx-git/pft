namespace Pft.Domain.ValueObjects;

public record Currency(string Code, string Symbol)
{
    public override string ToString() => $"{Code} ({Symbol})";
}