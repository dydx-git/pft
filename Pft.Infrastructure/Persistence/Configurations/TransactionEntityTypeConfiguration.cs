using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pft.Domain.Entities;

namespace Pft.Infrastructure.Persistence.Configurations;

public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Description).IsRequired().HasMaxLength(200);
        builder.OwnsOne(t => t.Amount);
        builder.Property(t => t.Date).IsRequired();
        builder.Property(t => t.Category).HasMaxLength(50);
        builder.HasMany(t => t.Tags).WithOne().HasForeignKey("Id");
        builder.OwnsOne(t => t.TransferFee);
        builder.OwnsOne(t => t.Recurrence);
    }
}