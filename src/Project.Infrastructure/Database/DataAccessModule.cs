using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Project.Application.Users;
using Project.Infrastructure.Domain.Users;
using Project.Infrastructure.SeedWork;
using Project.Infrastructure.Users;

namespace Project.Infrastructure.Database
{

    public static class DataAccessModule
    {
        public static IServiceCollection AddDataAccessModule(this IServiceCollection services)
        {
            var _databaseConnectionString =
             Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")
             ?? throw new InvalidOperationException("Environment variable DATABASE_CONNECTION_STRING is not set.");

            // Register DbContext
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(_databaseConnectionString, npgsqlOptions => { });

                // Strongly Typed Id Converter
                options.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
            });

            // Add Identity API EndPoints
            services.AddIdentityApiEndpoints<IdentityUserAdapter>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();


            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }

}
