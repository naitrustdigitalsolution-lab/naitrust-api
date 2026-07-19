using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class PaymentPartnerEventConfiguration : IEntityTypeConfiguration<PaymentPartnerEvent>
{
    public void Configure(EntityTypeBuilder<PaymentPartnerEvent> builder)
    {
        builder.ToTable("PaymentPartnerEvents");

        builder.Property(x => x.ProviderEventId).HasMaxLength(200).IsRequired();
        builder.Property(x => x.EventType).HasMaxLength(100).IsRequired();

        builder.Property(x => x.Partner).HasConversion<string>();

        builder.HasIndex(x => x.ProviderEventId).IsUnique();
        builder.HasIndex(x => x.VirtualAccountId);

        builder.HasOne<VirtualAccount>().WithMany().HasForeignKey(x => x.VirtualAccountId).OnDelete(DeleteBehavior.Cascade);
    }
}
