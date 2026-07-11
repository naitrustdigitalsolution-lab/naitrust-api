using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class EvidenceFileConfiguration : IEntityTypeConfiguration<EvidenceFile>
{
    public void Configure(EntityTypeBuilder<EvidenceFile> builder)
    {
        // TODO: Configure entity
    }
}
