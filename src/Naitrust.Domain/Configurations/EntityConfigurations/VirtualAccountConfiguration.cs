using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class VirtualAccountConfiguration : IEntityTypeConfiguration<VirtualAccount>
{
    public void Configure(EntityTypeBuilder<VirtualAccount> builder)
    {
        // TODO: Configure entity
    }
}
