using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pft.Domain.Entities.Accounts;

namespace Pft.Infrastructure.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasConversion(id => id.Value, value => new AccountId(value))
            .IsRequired();

        builder.OwnsOne(a => a.Currency, currencyBuilder =>
        {
            currencyBuilder.Property(c => c.Code).HasColumnName("CurrencyCode").IsRequired();
        });

        builder.OwnsOne(a => a.Balance, balanceBuilder =>
        {
            balanceBuilder.Property(b => b.Amount).HasColumnName("BalanceAmount").IsRequired();
            balanceBuilder.Property(b => b.Currency).HasColumnName("BalanceCurrency").IsRequired();
        });

        builder.HasMany(a => a.Transactions)
            .WithOne()
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasDiscriminator(a => a.AccountType)
            .HasValue<CheckingAccount>(AccountType.Checking)
            .HasValue<SavingsAccount>(AccountType.Savings)
            .HasValue<WalletAccount>(AccountType.Wallet)
            .HasValue<CreditCardAccount>(AccountType.CreditCard);

        builder.HasBaseType<Account>();
    }
}

public class CheckingAccountConfiguration : IEntityTypeConfiguration<CheckingAccount>
{
    public void Configure(EntityTypeBuilder<CheckingAccount> builder)
    {
        builder.Property(ca => ca.HasDebitCard).IsRequired();
    }
}

public class SavingsAccountConfiguration : IEntityTypeConfiguration<SavingsAccount>
{
    public void Configure(EntityTypeBuilder<SavingsAccount> builder)
    {
        builder.Property(sa => sa.InterestRate).IsRequired();
        builder.Property(sa => sa.MinimumBalance).IsRequired();
        builder.Property(sa => sa.LastInterestCalculationDate).IsRequired();
    }
}

public class CreditCardAccountConfiguration : IEntityTypeConfiguration<CreditCardAccount>
{
    public void Configure(EntityTypeBuilder<CreditCardAccount> builder)
    {
        builder.Property(cca => cca.CreditLimit).IsRequired();
        builder.Property(cca => cca.AnnualPercentageRate).IsRequired();
        builder.Property(cca => cca.PaymentDueDate).IsRequired();
        builder.Property(cca => cca.MinimumPayment).IsRequired();
        builder.Property(cca => cca.StatementClosingDate).IsRequired();
    }
}

public class WalletAccountConfiguration : IEntityTypeConfiguration<WalletAccount>
{
    public void Configure(EntityTypeBuilder<WalletAccount> builder)
    {
    }
}
