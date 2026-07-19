using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class ReportedConcernConfiguration : IEntityTypeConfiguration<ReportedConcern>
{
    public void Configure(EntityTypeBuilder<ReportedConcern> builder)
    {
        builder.ToTable("ReportedConcerns");

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(320).IsRequired();
        builder.Property(x => x.Category).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(5000).IsRequired();
    }
}
