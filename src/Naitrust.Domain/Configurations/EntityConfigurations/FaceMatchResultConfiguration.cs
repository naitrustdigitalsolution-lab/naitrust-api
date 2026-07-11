using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class FaceMatchResultConfiguration : IEntityTypeConfiguration<FaceMatchResult>
{
    public void Configure(EntityTypeBuilder<FaceMatchResult> builder)
    {
        // TODO: Configure entity
    }
}
