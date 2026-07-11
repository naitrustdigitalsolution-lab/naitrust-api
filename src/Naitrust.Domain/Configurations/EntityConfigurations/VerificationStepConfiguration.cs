using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class VerificationStepConfiguration : IEntityTypeConfiguration<VerificationStep>
{
    public void Configure(EntityTypeBuilder<VerificationStep> builder)
    {
        // TODO: Configure entity
    }
}
