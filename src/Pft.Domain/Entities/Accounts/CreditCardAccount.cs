using Pft.Domain.Shared;

namespace Pft.Domain.Entities.Accounts;

public class CreditCardAccount : Account
{
    public decimal CreditLimit { get; private set; }
    public decimal AnnualPercentageRate { get; private set; }
    public DateTime PaymentDueDate { get; private set; }
    public Money MinimumPayment { get; private set; }
    public DateTime StatementClosingDate { get; private set; }
    public override AccountType AccountType => AccountType.CreditCard;

    public CreditCardAccount(
        AccountId id, 
        string accountName, 
        Currency currency, 
        Money balance,
        decimal creditLimit,
        decimal annualPercentageRate,
        DateTime paymentDueDate,
        Money minimumPayment) 
        : base(id, accountName, currency, balance)
    {
        CreditLimit = creditLimit;
        AnnualPercentageRate = annualPercentageRate;
        PaymentDueDate = paymentDueDate;
        MinimumPayment = minimumPayment;
        StatementClosingDate = paymentDueDate.AddDays(-21);
    }

    private CreditCardAccount() { }
} 