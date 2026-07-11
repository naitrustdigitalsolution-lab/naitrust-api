using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class AiPromptVersionConfiguration : IEntityTypeConfiguration<AiPromptVersion>
{
    public void Configure(EntityTypeBuilder<AiPromptVersion> builder)
    {
        // TODO: Configure entity
    }
}
