using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class IdempotencyKeyConfiguration : IEntityTypeConfiguration<IdempotencyKey>
{
    public void Configure(EntityTypeBuilder<IdempotencyKey> builder)
    {
        builder.ToTable("IdempotencyKeys");

        builder.Property(x => x.Key).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Scope).HasMaxLength(100);
        builder.Property(x => x.RequestHash).HasMaxLength(512);

        builder.HasIndex(x => new { x.Key, x.Scope }).IsUnique();
        builder.HasIndex(x => x.ExpiresAt);
    }
}
