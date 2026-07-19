using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class PartyConfiguration : IEntityTypeConfiguration<Party>
{
    public void Configure(EntityTypeBuilder<Party> builder)
    {
        builder.ToTable("Parties");

        builder.Property(x => x.PartyType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.DisplayName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.VerificationStatus).HasMaxLength(50);
        builder.Property(x => x.BvnReference).HasMaxLength(128);
        builder.Property(x => x.CacReference).HasMaxLength(128);

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.BusinessId);

        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne<ReputationProfile>().WithMany().HasForeignKey(x => x.ReputationProfileId).OnDelete(DeleteBehavior.SetNull);
    }
}
