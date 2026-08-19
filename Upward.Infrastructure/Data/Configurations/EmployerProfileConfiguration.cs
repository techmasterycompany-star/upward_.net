using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upward.Domain.Entities;

namespace Upward.Infrastructure.Data.Configurations;

public class EmployerProfileConfiguration : IEntityTypeConfiguration<EmployerProfile>
{
    public void Configure(EntityTypeBuilder<EmployerProfile> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CompanyName).IsRequired();

        builder.HasOne(x => x.User)
            .WithOne(x => x.EmployerProfile)
            .HasForeignKey<EmployerProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Jobs)
            .WithOne(x => x.Employer)
            .HasForeignKey(x => x.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Subscriptions)
            .WithOne(x => x.Employer)
            .HasForeignKey(x => x.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}