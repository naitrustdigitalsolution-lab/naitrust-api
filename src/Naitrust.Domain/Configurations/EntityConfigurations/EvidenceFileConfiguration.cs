using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class EvidenceFileConfiguration : IEntityTypeConfiguration<EvidenceFile>
{
    public void Configure(EntityTypeBuilder<EvidenceFile> builder)
    {
        builder.ToTable("EvidenceFiles");

        builder.Property(x => x.FileUrl).HasMaxLength(2048).IsRequired();
        builder.Property(x => x.FileName).HasMaxLength(500).IsRequired();
        builder.Property(x => x.MimeType).HasMaxLength(100).IsRequired();

        builder.Property(x => x.Type).HasConversion<string>();

        builder.HasIndex(x => x.TransactionId);
        builder.HasIndex(x => x.MilestoneId);

        builder.HasOne<Deal>().WithMany().HasForeignKey(x => x.TransactionId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Milestone>().WithMany().HasForeignKey(x => x.MilestoneId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.UploadedByUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
