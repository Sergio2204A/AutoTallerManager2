using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Auth;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");

        builder.Property(rt => rt.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("token");

        builder.HasIndex(rt => rt.Token).IsUnique();

        builder.Property(rt => rt.Expires)
            .IsRequired()
            .HasColumnName("expires");

        builder.Property(rt => rt.Created)
            .IsRequired()
            .HasColumnName("created");

        builder.Property(rt => rt.Revoked)
            .HasColumnName("revoked");

        builder.Property(rt => rt.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");

        builder.Property(rt => rt.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Ignore(rt => rt.IsExpired);
        builder.Ignore(rt => rt.IsActive);
    }
}
