using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Project.Application.Users;
using Project.Domain.SeedWork;
using Project.Domain.Users;
using Project.Infrastructure.Domain;
using Project.Infrastructure.Domain.Users;
using Project.Infrastructure.SeedWork;

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

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }

}
