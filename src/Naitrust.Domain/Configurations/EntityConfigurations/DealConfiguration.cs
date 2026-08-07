using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class DealConfiguration : IEntityTypeConfiguration<Deal>
{
    public void Configure(EntityTypeBuilder<Deal> builder)
    {
        builder.ToTable("Transactions");

        builder.Property(x => x.Reference).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Currency).HasMaxLength(3).IsRequired();
        builder.Property(x => x.UseCase).HasMaxLength(200);
        builder.Property(x => x.DeliveryDueDate).HasMaxLength(50);
        builder.Property(x => x.ReleaseConditions).HasMaxLength(2000);
        builder.Property(x => x.PreviousReference).HasMaxLength(50);

        builder.Property(x => x.PartyMode).HasConversion<string>();
        builder.Property(x => x.DealType).HasConversion<string>();
        builder.Property(x => x.Category).HasConversion<string>();
        builder.Property(x => x.Status).HasConversion<string>();
        builder.Property(x => x.PaymentStatus).HasConversion<string>();
        builder.Property(x => x.VerificationLevelRequired).HasConversion<string>();
        builder.Property(x => x.RiskLevel).HasConversion<string>();

        builder.HasIndex(x => x.Reference).IsUnique();
        builder.HasIndex(x => x.CreatedByUserId);
        builder.HasIndex(x => new { x.Status, x.PaymentStatus });
        builder.HasIndex(x => x.BusinessId);

        builder.HasOne<TransactionType>().WithMany().HasForeignKey(x => x.TransactionTypeId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne<Agreement>().WithMany().HasForeignKey(x => x.AgreementId).OnDelete(DeleteBehavior.SetNull);
    }
}
