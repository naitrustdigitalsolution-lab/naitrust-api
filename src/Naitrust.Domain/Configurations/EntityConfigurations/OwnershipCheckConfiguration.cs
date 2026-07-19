using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class OwnershipCheckConfiguration : IEntityTypeConfiguration<OwnershipCheck>
{
    public void Configure(EntityTypeBuilder<OwnershipCheck> builder)
    {
        builder.ToTable("OwnershipChecks");

        builder.Property(x => x.Method).HasConversion<string>();
        builder.Property(x => x.Status).HasConversion<string>();

        builder.HasIndex(x => new { x.VerificationRequestId, x.BusinessId }).IsUnique();

        builder.HasOne<VerificationRequest>().WithMany().HasForeignKey(x => x.VerificationRequestId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
