using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class OwnershipCheckConfiguration : IEntityTypeConfiguration<OwnershipCheck>
{
    public void Configure(EntityTypeBuilder<OwnershipCheck> builder)
    {
        // TODO: Configure entity
    }
}
