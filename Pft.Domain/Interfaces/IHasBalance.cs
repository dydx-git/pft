using Pft.Domain.ValueObjects;

namespace Pft.Domain.Interfaces;

public interface IHasBalance
{
    Money Balance { get; }
}