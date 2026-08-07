using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class MilestoneConfiguration : IEntityTypeConfiguration<Milestone>
{
    public void Configure(EntityTypeBuilder<Milestone> builder)
    {
        builder.ToTable("Milestones");

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();

        builder.Property(x => x.Status).HasConversion<string>();

        builder.Property(x => x.DealId).HasColumnName("TransactionId");

        builder.HasIndex(x => x.DealId);

        builder.HasOne<Deal>().WithMany().HasForeignKey(x => x.DealId).OnDelete(DeleteBehavior.Cascade);
    }
}
