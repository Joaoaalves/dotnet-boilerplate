using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Users;
using Project.Infrastructure.SeedWork;

namespace Project.Infrastructure.Domain.Users
{
    /// <summary>
    /// Entity configuration for <see cref="IdentityUserAdapter"/>.
    /// </summary>
    internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AspNetUsers");

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasConversion(new NameConverter())
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasConversion(new NameConverter())
                .HasMaxLength(100);
        }
    }
}
