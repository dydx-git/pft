using Pft.Domain.Abstractions;
using Pft.Domain.Entities.Accounts;
using Pft.Domain.Shared;

namespace Pft.Domain.Entities.Transactions;

public record TransactionAddedEvent(AccountId AccountId, TransactionId TransactionId, Money Amount) : IDomainEvent;

public record BalanceChangedEvent(AccountId AccountId, Money NewBalance) : IDomainEvent;

public record TransferCompletedEvent(TransactionId TransactionId, AccountId SourceAccountId, AccountId DestinationAccountId, Money Amount, Money TransferFee) : IDomainEvent;

public record RecurringTransactionScheduledEvent(TransactionId TransactionId, RecurringTransaction Recurrence) : IDomainEvent;
