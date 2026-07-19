using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class AiAssessmentConfiguration : IEntityTypeConfiguration<AiAssessment>
{
    public void Configure(EntityTypeBuilder<AiAssessment> builder)
    {
        builder.ToTable("AiAssessments");

        builder.Property(x => x.EntityType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Model).HasMaxLength(100).IsRequired();
        builder.Property(x => x.CreatedBy).HasMaxLength(100).IsRequired();

        builder.Property(x => x.Confidence).HasPrecision(5, 4);

        builder.Property(x => x.AssessmentType).HasConversion<string>();
        builder.Property(x => x.RiskLevel).HasConversion<string>();

        builder.HasIndex(x => new { x.EntityType, x.EntityId });
    }
}
