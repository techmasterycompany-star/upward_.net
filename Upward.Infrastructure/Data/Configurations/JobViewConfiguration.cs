using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upwork.Domain.Entities;

namespace Upwork.Infrastructure.Data.Configurations;

public class JobViewConfiguration : IEntityTypeConfiguration<JobView>
{
    public void Configure(EntityTypeBuilder<JobView> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ViewedAt).IsRequired();

        builder.HasOne(x => x.Job)
            .WithMany(x => x.JobViews)
            .HasForeignKey(x => x.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}