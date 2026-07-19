using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class VectorDocumentConfiguration : IEntityTypeConfiguration<VectorDocument>
{
    public void Configure(EntityTypeBuilder<VectorDocument> builder)
    {
        builder.ToTable("VectorDocuments");

        builder.Property(x => x.SourceType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.EmbeddingModel).HasMaxLength(100).IsRequired();

        builder.HasIndex(x => new { x.SourceType, x.SourceId });
    }
}
