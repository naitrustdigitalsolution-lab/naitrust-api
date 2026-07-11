using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class AiFeedbackConfiguration : IEntityTypeConfiguration<AiFeedback>
{
    public void Configure(EntityTypeBuilder<AiFeedback> builder)
    {
        // TODO: Configure entity
    }
}
