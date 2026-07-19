using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.Property(x => x.Action).HasMaxLength(32);
        builder.Property(x => x.EntityType).HasMaxLength(128);
        builder.Property(x => x.IpAddress).HasMaxLength(64);

        builder.HasIndex(x => x.EntityId);
        builder.HasIndex(x => new { x.EntityType, x.Action });
        builder.HasIndex(x => x.ActorUserId);
        builder.HasIndex(x => x.CreatedAt);
    }
}
