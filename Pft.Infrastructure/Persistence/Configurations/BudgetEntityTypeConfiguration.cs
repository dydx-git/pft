using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pft.Domain.Entities;

namespace Pft.Infrastructure.Persistence.Configurations;

public class BudgetEntityTypeConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Category).IsRequired().HasMaxLength(100);
        builder.OwnsOne(b => b.Limit);
        builder.Property(b => b.Period).IsRequired();
        builder.OwnsOne(b => b.Spent);
        builder.HasMany(b => b.Transactions).WithOne().HasForeignKey("Id");
    }
}