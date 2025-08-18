using FluentValidation;
using Project.Application.Users.Commands.RegisterUser;

namespace Project.Api.Extensions
{
    /// <summary>
    /// Provides extension methods to configure validations for the API using FluentValidation.
    /// </summary>
    public static class ValidationExtension
    {
        /// <summary>
        /// Adds validators for commands with FluentValidation to the service collection.
        /// </summary>
        /// <param name="services">The service collection to which validators will be added.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance, allowing method chaining.</returns>
        public static IServiceCollection AddValidations(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<RegisterUserCommand>();
            return services;
        }
    }
}