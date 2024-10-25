using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pft.Domain.Entities;

namespace Pft.Infrastructure.Persistence.Configurations;

public class CreditCardAccountEntityTypeConfiguration : IEntityTypeConfiguration<CreditCardAccount>
{
    public void Configure(EntityTypeBuilder<CreditCardAccount> builder)
    {
        builder.HasBaseType<Account>();
        builder.OwnsOne(a => a.CreditLimit);
    }
}