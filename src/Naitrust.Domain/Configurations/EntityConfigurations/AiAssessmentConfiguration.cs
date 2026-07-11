using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class AiAssessmentConfiguration : IEntityTypeConfiguration<AiAssessment>
{
    public void Configure(EntityTypeBuilder<AiAssessment> builder)
    {
        // TODO: Configure entity
    }
}
