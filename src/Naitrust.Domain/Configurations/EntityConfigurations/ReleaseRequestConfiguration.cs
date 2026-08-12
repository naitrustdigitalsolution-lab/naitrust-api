using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class ReleaseRequestConfiguration : IEntityTypeConfiguration<ReleaseRequest>
{
    public void Configure(EntityTypeBuilder<ReleaseRequest> builder)
    {
        builder.ToTable("ReleaseRequests");

        builder.Property(x => x.Provider).HasMaxLength(100);
        builder.Property(x => x.ProviderReference).HasMaxLength(200);

        builder.Property(x => x.Status).HasConversion<string>();

        builder.HasIndex(x => x.TransactionId);

        builder.HasOne<Deal>().WithMany().HasForeignKey(x => x.TransactionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.RequestedByUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
