using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Infrastructure.Users;

namespace Project.Infrastructure.Domain.Users
{
    /// <summary>
    /// Entity configuration for <see cref="IdentityUserAdapter"/>.
    /// </summary>
    internal sealed class UserTypeConfiguration : IEntityTypeConfiguration<IdentityUserAdapter>
    {
        public void Configure(EntityTypeBuilder<IdentityUserAdapter> builder)
        {
            builder.ToTable("AspNetUsers");

            // Key configuration with TypedId conversion
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .ValueGeneratedNever();

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.UserName).IsUnique();
        }
    }
}
