using Pft.Domain.Entities;
using Pft.Domain.ValueObjects;

namespace Pft.Domain.Specifications;

public class CreditLimitSpecification
{
    public bool IsSatisfiedBy(CreditCardAccount account, Money amount)
    {
        return account.CanMakePurchase(amount);
    }
}