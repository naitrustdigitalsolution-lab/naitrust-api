using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class TransactionPartyConfiguration : IEntityTypeConfiguration<TransactionParty>
{
    public void Configure(EntityTypeBuilder<TransactionParty> builder)
    {
        // TODO: Configure entity
    }
}
