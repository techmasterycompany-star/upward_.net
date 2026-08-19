using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upward.Domain.Entities;

namespace Upward.Infrastructure.Data.Configurations;

public class JobTechnologyConfiguration : IEntityTypeConfiguration<JobTechnology>
{
    public void Configure(EntityTypeBuilder<JobTechnology> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.JobId, x.TechnologyId }).IsUnique();

        builder.HasOne(x => x.Job)
            .WithMany(x => x.JobTechnologies)
            .HasForeignKey(x => x.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Technology)
            .WithMany(x => x.JobTechnologies)
            .HasForeignKey(x => x.TechnologyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}