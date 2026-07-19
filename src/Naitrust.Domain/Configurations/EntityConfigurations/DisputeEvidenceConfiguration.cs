using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class DisputeEvidenceConfiguration : IEntityTypeConfiguration<DisputeEvidence>
{
    public void Configure(EntityTypeBuilder<DisputeEvidence> builder)
    {
        builder.ToTable("DisputeEvidence");

        builder.HasIndex(x => new { x.DisputeId, x.EvidenceFileId }).IsUnique();

        builder.HasOne<Dispute>().WithMany().HasForeignKey(x => x.DisputeId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<EvidenceFile>().WithMany().HasForeignKey(x => x.EvidenceFileId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.SubmittedByUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
