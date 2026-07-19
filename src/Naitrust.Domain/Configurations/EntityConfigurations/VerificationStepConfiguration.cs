using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class VerificationStepConfiguration : IEntityTypeConfiguration<VerificationStep>
{
    public void Configure(EntityTypeBuilder<VerificationStep> builder)
    {
        builder.ToTable("VerificationSteps");

        builder.Property(x => x.Provider).HasMaxLength(100);

        builder.Property(x => x.Step).HasConversion<string>();
        builder.Property(x => x.Status).HasConversion<string>();

        builder.HasIndex(x => new { x.VerificationRequestId, x.Step }).IsUnique();

        builder.HasOne<VerificationRequest>().WithMany().HasForeignKey(x => x.VerificationRequestId).OnDelete(DeleteBehavior.Cascade);
    }
}
