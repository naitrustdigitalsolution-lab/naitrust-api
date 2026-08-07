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
        builder.Property(x => x.Slug).HasMaxLength(250);
        builder.Property(x => x.NtId).HasMaxLength(20);
        builder.Property(x => x.Country).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Type).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.OwnerName).HasMaxLength(200);
        builder.Property(x => x.Email).HasMaxLength(320);
        builder.Property(x => x.Phone).HasMaxLength(20);
        builder.Property(x => x.Website).HasMaxLength(500);
        builder.Property(x => x.RegistrationNumber).HasMaxLength(50);
        builder.Property(x => x.TaxId).HasMaxLength(50);
        builder.Property(x => x.State).HasMaxLength(100);
        builder.Property(x => x.City).HasMaxLength(100);
        builder.Property(x => x.Address).HasMaxLength(500);
        builder.Property(x => x.SocialHandles).HasMaxLength(2000);
        builder.Property(x => x.PaymentAccountBankName).HasMaxLength(200);
        builder.Property(x => x.PaymentAccountNumber).HasMaxLength(50);
        builder.Property(x => x.PaymentAccountName).HasMaxLength(200);

        builder.Property(x => x.VerificationStatus).HasConversion<string>();
        builder.Property(x => x.RiskLevel).HasConversion<string>();

        builder.HasIndex(x => x.OwnerUserId);
        builder.HasIndex(x => x.Slug).IsUnique().HasFilter("\"Slug\" IS NOT NULL");
        builder.HasIndex(x => x.NtId).IsUnique().HasFilter("\"NtId\" IS NOT NULL");
        builder.HasIndex(x => x.RegistrationNumber);
        builder.HasIndex(x => x.VerificationStatus);

        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.OwnerUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
