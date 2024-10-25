using Pft.Domain.Interfaces;
using Pft.Domain.ValueObjects;

namespace Pft.Domain.Entities;

public record Transaction(Guid Id, Account RelatedAccount, string Description, Money Amount, TransactionType Type, DateTime Date, string? Category, List<string> Tags, RecurringTransaction? Recurrence, Money? TransferFee) : IEntity
{
    public bool IsRecurring() => Recurrence is not null;
    public override string ToString() => $"{Description} ({Type}) - {Amount}";
}