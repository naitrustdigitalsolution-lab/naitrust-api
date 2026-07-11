using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class VectorDocumentConfiguration : IEntityTypeConfiguration<VectorDocument>
{
    public void Configure(EntityTypeBuilder<VectorDocument> builder)
    {
        // TODO: Configure entity
    }
}
