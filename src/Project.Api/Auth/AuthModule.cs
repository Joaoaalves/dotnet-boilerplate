using Microsoft.AspNetCore.Identity;
using Project.Domain.Users;
using Project.Infrastructure.Database;

namespace Project.Api.Auth
{
    /// <summary>
    /// Provides extension methods for configuring authentication and identity services for the application.
    /// </summary>
    public static class AuthModule
    {
        /// <summary>
        /// Registers and configures authentication services, including Identity, token providers,
        /// and custom logging implementations for <see cref="UserManager{TUser}"/> and <see cref="SignInManager{TUser}"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which authentication services will be added.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        /// <remarks>
        /// This method performs the following:
        /// <list type="bullet">
        ///   <item>Adds Identity API endpoints for the custom <see cref="User"/> entity.</item>
        ///   <item>Registers the <see cref="ApplicationDbContext"/> as the data store for Identity.</item>
        ///   <item>Adds default token providers used for password resets, email confirmation, etc.</item>
        ///   <item>Removes the default <see cref="SignInManager{User}"/> registration to allow custom behavior.</item>
        ///   <item>Registers custom logging implementations of <see cref="SignInManager{User}"/> and <see cref="UserManager{User}"/>.</item>
        /// </list>
        /// </remarks>
        public static IServiceCollection AddAuthModule(this IServiceCollection services)
        {
            services
                .AddIdentityApiEndpoints<User>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Remove default SignInManager
            services.Remove(
                services.Single(s => s.ServiceType == typeof(SignInManager<User>))
            );

            // Add Logging versions
            services.AddScoped<SignInManager<User>, LoggingSignInManager>();
            services.AddScoped<UserManager<User>, LoggingUserManager>();

            return services;
        }
    }
}
