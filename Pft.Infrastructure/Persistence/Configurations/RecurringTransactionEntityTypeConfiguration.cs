using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pft.Domain.ValueObjects;

namespace Pft.Infrastructure.Persistence.Configurations;

public class RecurringTransactionEntityTypeConfiguration : IEntityTypeConfiguration<RecurringTransaction>
{
    public void Configure(EntityTypeBuilder<RecurringTransaction> builder)
    {
        builder.HasKey(r => r.RecurrenceId);
        builder.Property(r => r.Frequency).IsRequired();
        builder.Property(r => r.StartDate).IsRequired();
        builder.Property(r => r.EndDate);
    }
}