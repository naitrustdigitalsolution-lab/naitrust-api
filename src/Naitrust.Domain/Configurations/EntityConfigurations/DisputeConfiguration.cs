using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class DisputeConfiguration : IEntityTypeConfiguration<Dispute>
{
    public void Configure(EntityTypeBuilder<Dispute> builder)
    {
        builder.ToTable("Disputes");

        builder.Property(x => x.Reason).HasMaxLength(200).IsRequired();
        builder.Property(x => x.HasEvidence).HasDefaultValue(false);

        builder.Property(x => x.Status).HasConversion<string>();
        builder.Property(x => x.Resolution).HasConversion<string>();

        builder.Property(x => x.DealId).HasColumnName("TransactionId");

        builder.HasIndex(x => x.DealId);
        builder.HasIndex(x => x.OpenedByUserId);

        builder.HasOne<Deal>().WithMany().HasForeignKey(x => x.DealId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.OpenedByUserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.AdminOwnerId).OnDelete(DeleteBehavior.SetNull);
    }
}
