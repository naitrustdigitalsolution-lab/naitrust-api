using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class DealInvitationConfiguration : IEntityTypeConfiguration<DealInvitation>
{
    public void Configure(EntityTypeBuilder<DealInvitation> builder)
    {
        builder.ToTable("DealInvitations");

        builder.Property(x => x.PublicToken).HasMaxLength(100).IsRequired();
        builder.Property(x => x.IntendedContact).HasMaxLength(320);
        builder.Property(x => x.IntendedAccountType).HasMaxLength(50);
        builder.Property(x => x.PostAuthDestination).HasMaxLength(500);
        builder.Property(x => x.FromName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.FromRole).HasMaxLength(50).IsRequired();
        builder.Property(x => x.YourRole).HasMaxLength(50).IsRequired();
        builder.Property(x => x.PartyMode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Currency).HasMaxLength(3).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(2000);
        builder.Property(x => x.AgreementSnapshot).HasColumnType("text");

        builder.Property(x => x.Status).HasConversion<string>();

        builder.Property(x => x.DealId).HasColumnName("TransactionId");

        builder.HasIndex(x => x.PublicToken).IsUnique();
        builder.HasIndex(x => x.DealId);
        builder.HasIndex(x => x.RecipientUserId);
        builder.HasIndex(x => x.Status);

        builder.HasOne<Deal>().WithMany().HasForeignKey(x => x.DealId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.RecipientUserId).OnDelete(DeleteBehavior.SetNull);
    }
}
