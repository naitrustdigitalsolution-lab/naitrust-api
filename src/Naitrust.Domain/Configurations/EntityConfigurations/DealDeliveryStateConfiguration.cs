using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class DealDeliveryStateConfiguration : IEntityTypeConfiguration<DealDeliveryState>
{
    public void Configure(EntityTypeBuilder<DealDeliveryState> builder)
    {
        builder.ToTable("DealDeliveryStates");

        builder.Property(x => x.CardToken).HasMaxLength(64);
        builder.Property(x => x.CardOtpCode).HasMaxLength(6);
        builder.Property(x => x.PaymentReference).HasMaxLength(100);

        builder.Property(x => x.CardStatus).HasConversion<string>();
        builder.Property(x => x.HandoverStatus).HasConversion<string>();
        builder.Property(x => x.HandoverCompletionReason).HasConversion<string>();
        builder.Property(x => x.FundingReviewStatus).HasConversion<string>();
        builder.Property(x => x.ReleaseMethod).HasConversion<string>();

        builder.Property(x => x.DealId).HasColumnName("TransactionId");

        builder.HasIndex(x => x.DealId).IsUnique();
        builder.HasIndex(x => x.CardToken);

        builder.HasOne<Deal>().WithMany().HasForeignKey(x => x.DealId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.CardIntendedBuyerUserId).OnDelete(DeleteBehavior.SetNull);
    }
}
