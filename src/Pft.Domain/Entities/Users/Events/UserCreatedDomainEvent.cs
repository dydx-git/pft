using Pft.Domain.Abstractions;

namespace Pft.Domain.Entities.Users.Events;

public sealed record UserCreatedDomainEvent(UserId UserId) : IDomainEvent;
