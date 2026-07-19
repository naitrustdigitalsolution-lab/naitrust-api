using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class VerificationProviderEventConfiguration : IEntityTypeConfiguration<VerificationProviderEvent>
{
    public void Configure(EntityTypeBuilder<VerificationProviderEvent> builder)
    {
        builder.ToTable("VerificationProviderEvents");

        builder.Property(x => x.Provider).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ProviderReference).HasMaxLength(200).IsRequired();
        builder.Property(x => x.EventType).HasMaxLength(100).IsRequired();

        builder.HasIndex(x => x.VerificationRequestId);
        builder.HasIndex(x => x.ProviderReference);

        builder.HasOne<VerificationRequest>().WithMany().HasForeignKey(x => x.VerificationRequestId).OnDelete(DeleteBehavior.Cascade);
    }
}
