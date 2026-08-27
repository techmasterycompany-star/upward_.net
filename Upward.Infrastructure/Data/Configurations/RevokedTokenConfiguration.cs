using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upward.Domain.Entities;

namespace Upward.Infrastructure.Data.Configurations;

public class RevokedTokenConfiguration : IEntityTypeConfiguration<RevokedToken>
{
    public void Configure(EntityTypeBuilder<RevokedToken> builder)
    {
        builder.ToTable("RevokedTokens");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Jti)
            .HasMaxLength(64)
            .IsRequired();

        builder.HasIndex(t => t.Jti)
            .IsUnique();

        builder.Property(t => t.ExpiresAt)
            .IsRequired();
    }
}