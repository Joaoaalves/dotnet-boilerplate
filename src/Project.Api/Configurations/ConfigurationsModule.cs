using Project.API.Configurations;
using Project.Application.Configuration;

namespace Project.Api.Configurations
{
    public static class ConfigurationsModule
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services)
        {
            services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();
            return services;
        }
    }
}