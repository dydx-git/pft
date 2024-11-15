using Pft.Domain.Shared;

namespace Pft.Domain.Entities.Accounts;

public interface IHasBalance
{
    Money Balance { get; }
}