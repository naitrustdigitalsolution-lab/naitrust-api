using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();

        builder.Property(x => x.Type).HasConversion<string>();

        builder.HasIndex(x => new { x.UserId, x.ReadAt });
        builder.HasIndex(x => new { x.UserId, x.CreatedAt });

        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
