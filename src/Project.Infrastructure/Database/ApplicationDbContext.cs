using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Infrastructure.Users;

namespace Project.Infrastructure.Database
{
    /// <summary>
    /// Represents the Entity Framework Core database context for the application,
    /// integrating with ASP.NET Identity and custom domain configurations.
    /// </summary>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<IdentityUserAdapter>(options)
    {
        /// <summary>
        /// Configures the entity models and applies all configurations found in the assembly.
        /// </summary>
        /// <param name="builder">The model builder used to configure entity mappings.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
