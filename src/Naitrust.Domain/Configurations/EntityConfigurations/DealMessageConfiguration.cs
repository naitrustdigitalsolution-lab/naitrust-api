using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class DealMessageConfiguration : IEntityTypeConfiguration<DealMessage>
{
    public void Configure(EntityTypeBuilder<DealMessage> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.DealId).HasColumnName("TransactionId").IsRequired();
        builder.Property(m => m.SenderUserId).IsRequired();
        builder.Property(m => m.SenderName).IsRequired().HasMaxLength(200);
        builder.Property(m => m.Body).IsRequired();
        builder.HasIndex(m => m.DealId);
    }
}
