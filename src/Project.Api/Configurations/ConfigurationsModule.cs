using Project.API.Configurations;
using Project.Application.Configuration;

namespace Project.Api.Configurations
{
    /// <summary>
    /// Provides extension methods to register configuration-related services in the application's dependency injection container.
    /// </summary>
    public static class ConfigurationsModule
    {
        /// <summary>
        /// Registers application configuration services such as the <see cref="IExecutionContextAccessor"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the services will be added.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        /// <remarks>
        /// This method ensures that infrastructure-level dependencies used for accessing execution context 
        /// (e.g., current user or request context) are properly registered.
        /// </remarks>
        public static IServiceCollection AddConfigurations(this IServiceCollection services)
        {
            services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();
            return services;
        }
    }
}
