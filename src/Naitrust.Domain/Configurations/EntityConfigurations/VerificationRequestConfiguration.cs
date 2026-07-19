using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class VerificationRequestConfiguration : IEntityTypeConfiguration<VerificationRequest>
{
    public void Configure(EntityTypeBuilder<VerificationRequest> builder)
    {
        builder.ToTable("VerificationRequests");

        builder.Property(x => x.Provider).HasMaxLength(100);
        builder.Property(x => x.PaymentStatus).HasMaxLength(50);
        builder.Property(x => x.PaymentReference).HasMaxLength(200);
        builder.Property(x => x.ProviderReference).HasMaxLength(200);

        builder.Property(x => x.SubjectType).HasConversion<string>();
        builder.Property(x => x.VerificationType).HasConversion<string>();
        builder.Property(x => x.VerificationLevel).HasConversion<string>();
        builder.Property(x => x.Status).HasConversion<string>();

        builder.HasIndex(x => new { x.SubjectType, x.SubjectId });
        builder.HasIndex(x => x.TransactionId);
        builder.HasIndex(x => x.RequestedByUserId);

        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.RequestedByUserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Transaction>().WithMany().HasForeignKey(x => x.TransactionId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.ReviewedBy).OnDelete(DeleteBehavior.SetNull);
    }
}
