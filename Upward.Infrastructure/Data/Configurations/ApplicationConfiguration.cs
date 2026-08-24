using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upward.Domain.Entities;

namespace Upward.Infrastructure.Data.Configurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("Applications");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Resume).IsRequired();
        builder.Property(x => x.ContactEmail).IsRequired();
        builder.Property(x => x.ContactPhone).IsRequired();
        builder.Property(x => x.Status).IsRequired();

        builder.HasOne(x => x.Job)
            .WithMany(x => x.Applications)
            .HasForeignKey(x => x.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Candidate)
            .WithMany(x => x.Applications)
            .HasForeignKey(x => x.CandidateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}