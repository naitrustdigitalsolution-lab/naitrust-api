using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class BusinessConfiguration : IEntityTypeConfiguration<Business>
{
    public void Configure(EntityTypeBuilder<Business> builder)
    {
        builder.ToTable("Businesses");

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Country).HasMaxLength(3).IsRequired();
        builder.Property(x => x.Type).HasMaxLength(100);
        builder.Property(x => x.RegistrationNumber).HasMaxLength(50);
        builder.Property(x => x.TaxId).HasMaxLength(50);
        builder.Property(x => x.State).HasMaxLength(100);
        builder.Property(x => x.Address).HasMaxLength(500);

        builder.Property(x => x.VerificationStatus).HasConversion<string>();
        builder.Property(x => x.RiskLevel).HasConversion<string>();

        builder.HasIndex(x => x.OwnerUserId);
        builder.HasIndex(x => x.RegistrationNumber);
        builder.HasIndex(x => x.VerificationStatus);

        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.OwnerUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
