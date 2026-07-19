using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class AiPromptVersionConfiguration : IEntityTypeConfiguration<AiPromptVersion>
{
    public void Configure(EntityTypeBuilder<AiPromptVersion> builder)
    {
        builder.ToTable("AiPromptVersions");

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

        builder.Property(x => x.Status).HasConversion<string>();

        builder.HasIndex(x => new { x.Name, x.Version }).IsUnique();
    }
}
