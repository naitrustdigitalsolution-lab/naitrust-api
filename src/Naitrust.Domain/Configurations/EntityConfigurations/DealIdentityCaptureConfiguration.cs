using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class DealIdentityCaptureConfiguration : IEntityTypeConfiguration<DealIdentityCapture>
{
    public void Configure(EntityTypeBuilder<DealIdentityCapture> builder)
    {
        builder.ToTable("DealIdentityCaptures");

        builder.Property(x => x.RepresentativeName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.BusinessName).HasMaxLength(200);
        builder.Property(x => x.EncryptedEvidenceRef).HasMaxLength(1000);
        builder.Property(x => x.ClientCaptureId).HasMaxLength(200);

        builder.Property(x => x.Action).HasConversion<string>();
        builder.Property(x => x.VerificationStatus).HasConversion<string>();

        builder.Property(x => x.DealId).HasColumnName("TransactionId");

        builder.HasIndex(x => x.DealId);
        builder.HasIndex(x => x.SubjectUserId);

        builder.HasOne<Deal>().WithMany().HasForeignKey(x => x.DealId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.SubjectUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
