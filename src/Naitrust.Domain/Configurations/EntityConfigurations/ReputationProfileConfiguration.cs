using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class ReputationProfileConfiguration : IEntityTypeConfiguration<ReputationProfile>
{
    public void Configure(EntityTypeBuilder<ReputationProfile> builder)
    {
        builder.ToTable("ReputationProfiles");

        builder.Property(x => x.RatingAverage).HasPrecision(5, 2);

        builder.Property(x => x.SubjectType).HasConversion<string>();

        builder.HasIndex(x => new { x.SubjectType, x.SubjectId }).IsUnique();
    }
}
