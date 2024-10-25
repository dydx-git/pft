using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pft.Domain.Entities;

namespace Pft.Infrastructure.Persistence.Configurations;

public class SavingsAccountEntityTypeConfiguration : IEntityTypeConfiguration<SavingsAccount>
{
    public void Configure(EntityTypeBuilder<SavingsAccount> builder)
    {
        builder.HasBaseType<Account>();
    }
}