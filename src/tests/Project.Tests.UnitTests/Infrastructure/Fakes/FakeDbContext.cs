using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Domain.Users;
using Project.Tests.UnitTests.Domain.Fakes;

namespace Project.Tests.UnitTests.Infrastructure.Fakes
{
    public class FakeDbContext<TUser>(DbContextOptions<FakeDbContext<TUser>> options) : IdentityDbContext<TUser>(options) where TUser : IdentityUser
    {
        public DbSet<FakeEntity> FakeEntities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User to own Name
            modelBuilder.Entity<User>(builder =>
            {
                builder.OwnsOne(u => u.FirstName);
                builder.OwnsOne(u => u.LastName);
            });
        }
    }

}