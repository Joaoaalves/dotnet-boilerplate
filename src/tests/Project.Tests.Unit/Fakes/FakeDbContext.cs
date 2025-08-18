using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Domain.Users;

namespace Project.Tests.Unit.Fakes
{
    public class FakeDbContext<TUser>(DbContextOptions<FakeDbContext<TUser>> options) : IdentityDbContext<TUser>(options) where TUser : IdentityUser
    {
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