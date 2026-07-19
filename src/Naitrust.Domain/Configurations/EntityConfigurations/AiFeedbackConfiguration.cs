using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Domain.Configurations.EntityConfigurations;

public class AiFeedbackConfiguration : IEntityTypeConfiguration<AiFeedback>
{
    public void Configure(EntityTypeBuilder<AiFeedback> builder)
    {
        builder.ToTable("AiFeedbacks");

        builder.Property(x => x.FeedbackType).HasConversion<string>();

        builder.HasIndex(x => x.AssessmentId);

        builder.HasOne<AiAssessment>().WithMany().HasForeignKey(x => x.AssessmentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<NaitrustUser>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
