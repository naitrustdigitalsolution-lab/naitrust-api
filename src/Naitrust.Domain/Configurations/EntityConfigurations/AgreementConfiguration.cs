using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class AgreementConfiguration : IEntityTypeConfiguration<Agreement>
{
    public void Configure(EntityTypeBuilder<Agreement> builder)
    {
        builder.ToTable("Agreements");

        builder.Property(x => x.SectionsJson).HasColumnType("text");

        builder.Property(x => x.DealId).HasColumnName("TransactionId");

        builder.HasIndex(x => new { x.DealId, x.Version }).IsUnique();

        builder.HasOne<Deal>().WithMany().HasForeignKey(x => x.DealId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
