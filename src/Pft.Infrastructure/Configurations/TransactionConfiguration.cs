using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pft.Domain.Entities.Accounts;
using Pft.Domain.Entities.Transactions;

namespace Pft.Infrastructure.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(id => id.Value, value => new TransactionId(value))
            .IsRequired();

        builder.Property(t => t.Date).IsRequired();
        builder.OwnsOne(t => t.Amount, amountBuilder =>
        {
            amountBuilder.Property(a => a.Amount).HasColumnName("Amount").IsRequired();
            amountBuilder.Property(a => a.Currency).HasColumnName("Currency").IsRequired();
        });

        builder.Property(t => t.Tags)
            .HasField("Tags")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsOne(t => t.Recurrence, recurrenceBuilder =>
        {
            recurrenceBuilder.Property(r => r.RecurrenceId).HasColumnName("RecurrenceId");
            recurrenceBuilder.Property(r => r.Frequency).HasColumnName("Frequency").IsRequired();
            recurrenceBuilder.Property(r => r.StartDate).HasColumnName("StartDate").IsRequired();
            recurrenceBuilder.Property(r => r.EndDate).HasColumnName("EndDate");
        });

        builder.HasOne<Account>()
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasDiscriminator<string>("TransactionType")
            .HasValue<ExpenseTransaction>("Expense")
            .HasValue<IncomeTransaction>("Income")
            .HasValue<TransferTransaction>("Transfer");
    }
}

public class ExpenseTransactionConfiguration : IEntityTypeConfiguration<ExpenseTransaction>
{
    public void Configure(EntityTypeBuilder<ExpenseTransaction> builder)
    {
        builder.Property(et => et.Category).IsRequired();
    }
}

public class IncomeTransactionConfiguration : IEntityTypeConfiguration<IncomeTransaction>
{
    public void Configure(EntityTypeBuilder<IncomeTransaction> builder)
    {
        builder.Property(it => it.Category).IsRequired();
    }
}

public class TransferTransactionConfiguration : IEntityTypeConfiguration<TransferTransaction>
{
    public void Configure(EntityTypeBuilder<TransferTransaction> builder)
    {
        builder.Property(tt => tt.DestinationAccountId)
            .HasConversion(id => id.Value, value => new AccountId(value))
            .IsRequired();
        
        builder.OwnsOne(tt => tt.TransferFee, feeBuilder =>
        {
            feeBuilder.Property(f => f.Amount).HasColumnName("TransferFeeAmount").IsRequired();
            feeBuilder.Property(f => f.Currency).HasColumnName("TransferFeeCurrency").IsRequired();
        });
    }
}
