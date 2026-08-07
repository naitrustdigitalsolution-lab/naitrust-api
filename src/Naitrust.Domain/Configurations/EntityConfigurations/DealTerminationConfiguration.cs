using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class DealTerminationConfiguration : IEntityTypeConfiguration<DealTermination>
{
    public void Configure(EntityTypeBuilder<DealTermination> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.DealId).HasColumnName("TransactionId").IsRequired();
        builder.Property(t => t.RequestedByUserId).IsRequired();
        builder.Property(t => t.Reason).IsRequired();
        builder.Property(t => t.Status).IsRequired().HasMaxLength(50);
        builder.HasIndex(t => t.DealId);
    }
}
