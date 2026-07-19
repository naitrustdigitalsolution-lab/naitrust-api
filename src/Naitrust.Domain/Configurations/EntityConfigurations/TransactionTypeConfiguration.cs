using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionType>
{
    public void Configure(EntityTypeBuilder<TransactionType> builder)
    {
        builder.ToTable("TransactionTypes");

        builder.Property(x => x.Key).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();

        builder.Property(x => x.RequiredVerificationLevel).HasConversion<string>();
        builder.Property(x => x.ReleaseMode).HasConversion<string>();

        builder.HasIndex(x => x.Key).IsUnique();
    }
}
