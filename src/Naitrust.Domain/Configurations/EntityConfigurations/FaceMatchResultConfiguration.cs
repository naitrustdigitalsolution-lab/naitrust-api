using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class FaceMatchResultConfiguration : IEntityTypeConfiguration<FaceMatchResult>
{
    public void Configure(EntityTypeBuilder<FaceMatchResult> builder)
    {
        builder.ToTable("FaceMatchResults");

        builder.Property(x => x.Provider).HasMaxLength(100).IsRequired();
        builder.Property(x => x.IdNumberHash).HasMaxLength(512).IsRequired();

        builder.Property(x => x.MatchScore).HasPrecision(5, 4);
        builder.Property(x => x.Confidence).HasPrecision(5, 4);

        builder.Property(x => x.IdType).HasConversion<string>();

        builder.HasIndex(x => x.VerificationRequestId);

        builder.HasOne<VerificationRequest>().WithMany().HasForeignKey(x => x.VerificationRequestId).OnDelete(DeleteBehavior.Cascade);
    }
}
