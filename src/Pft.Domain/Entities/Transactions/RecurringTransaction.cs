using Pft.Domain.Shared;

namespace Pft.Domain.Entities.Transactions;

public record RecurringTransaction(Guid RecurrenceId, RecurrenceFrequency Frequency, DateTime StartDate, DateTime? EndDate)
{
    public bool IsOngoing() => !EndDate.HasValue || EndDate > DateTime.Now;
}