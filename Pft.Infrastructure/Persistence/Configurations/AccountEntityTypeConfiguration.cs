using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pft.Domain.Entities;

namespace Pft.Infrastructure.Persistence.Configurations;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.AccountName).IsRequired().HasMaxLength(100);
        builder.OwnsOne(a => a.Balance);
        builder.HasMany(a => a.Transactions).WithOne().HasForeignKey("Id");
        builder.HasDiscriminator<string>("AccountType").HasValue<CheckingAccount>("Checking").HasValue<SavingsAccount>("Savings").HasValue<CreditCardAccount>("CreditCard");
    }
}