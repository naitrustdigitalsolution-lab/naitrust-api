using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class ReleaseRequestConfiguration : IEntityTypeConfiguration<ReleaseRequest>
{
    public void Configure(EntityTypeBuilder<ReleaseRequest> builder)
    {
        // TODO: Configure entity
    }
}
