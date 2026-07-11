using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class PaymentPartnerEventConfiguration : IEntityTypeConfiguration<PaymentPartnerEvent>
{
    public void Configure(EntityTypeBuilder<PaymentPartnerEvent> builder)
    {
        // TODO: Configure entity
    }
}
