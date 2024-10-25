using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pft.Domain.Entities;

namespace Pft.Infrastructure.Persistence.Configurations;

public class CheckingAccountEntityTypeConfiguration : IEntityTypeConfiguration<CheckingAccount>
{
    public void Configure(EntityTypeBuilder<CheckingAccount> builder)
    {
        builder.HasBaseType<Account>();
    }
}