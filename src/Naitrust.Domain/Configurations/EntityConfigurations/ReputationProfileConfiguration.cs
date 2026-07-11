using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class ReputationProfileConfiguration : IEntityTypeConfiguration<ReputationProfile>
{
    public void Configure(EntityTypeBuilder<ReputationProfile> builder)
    {
        // TODO: Configure entity
    }
}
