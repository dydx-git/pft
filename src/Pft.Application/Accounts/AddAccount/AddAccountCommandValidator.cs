using FluentValidation;

namespace Pft.Application.Accounts.AddAccount;

public sealed class AddAccountCommandValidator : AbstractValidator<AddAccountCommand>
{
    public AddAccountCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.CurrencyCode).NotEmpty();
        RuleFor(c => c.AccountType).IsInEnum();
        RuleFor(c => c.Balance).GreaterThanOrEqualTo(0);
    }    
}