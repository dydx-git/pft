using Pft.Application.Abstractions.Messaging;
using Pft.Domain.Entities.Accounts;

namespace Pft.Application.Accounts.AddAccount;

public record AddAccountCommand(
    string Name,
    string CurrencyCode,
    AccountType AccountType,
    decimal Balance) : ICommand<Guid>;