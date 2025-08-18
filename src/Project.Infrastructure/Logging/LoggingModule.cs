using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;
using Project.Infrastructure.Logging.Users;
using Project.Logging.Users;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Project.Infrastructure.Logging
{
    /// <summary>
    /// Provides methods to configure and register logging services, including Serilog and Elasticsearch integration.
    /// </summary>
    public static class LoggingModule
    {
        /// <summary>
        /// Registers services related to user logging into the application's dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to register services into.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        /// <remarks>
        /// Registers a default or Elasticsearch-based implementation of <see cref="IUserLoggerService"/>,
        /// based on the ENABLE_LOGGING environment variable.
        /// </remarks>
        public static IServiceCollection AddLogginModule(this IServiceCollection services, IHostEnvironment env)
        {
            services.AddSingleton<UserLogIndexDefinition>();

            var isLogginEnabled = Environment.GetEnvironmentVariable("ENABLE_LOGGING");

            if (env.IsEnvironment("Testing") || isLogginEnabled is null || isLogginEnabled != "true")
            {
                services.AddSingleton<IUserLoggerService, DefaultUserLoggerService>();
                return services;
            }

            var elasticUri = Environment.GetEnvironmentVariable("ELASTICSEARCH_URL")
                ?? throw new Exception("You must provide ELASTICSEARCH_URL param, otherwise you should disable Logging.");

            services.AddSingleton<IElasticClient>(sp =>
            {
                var settings = new ConnectionSettings(new Uri(elasticUri))
                    .DefaultIndex("logs")
                    .ServerCertificateValidationCallback((o, certificate, chain, errors) => true);

                return new ElasticClient(settings);
            });

            services.AddSingleton<IUserLoggerService, ElasticUserLoggerService>();

            return services;
        }

        /// <summary>
        /// Configures Serilog and Elasticsearch sinks during application startup.
        /// </summary>
        /// <param name="hostBuilder">The host builder to apply logging configuration to.</param>
        /// <remarks>
        /// Uses environment variables to control whether logging is enabled and to determine the Elasticsearch URL.
        /// </remarks>
        public static void SetupLoggingModule(this IHostBuilder hostBuilder)
        {
            var isLogginEnabled = Environment.GetEnvironmentVariable("ENABLE_LOGGING");

            if (isLogginEnabled is null)
                return;

            if (isLogginEnabled == "true")
            {
                var elasticUri = Environment.GetEnvironmentVariable("ELASTICSEARCH_URL")
                    ?? throw new Exception("You must provide ELASTICSEARCH_URL param, otherwise you should disable Logging.");

                hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithThreadId()
                        .Enrich.WithProcessId()
                        .WriteTo.Console()
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                        {
                            AutoRegisterTemplate = true,
                            IndexFormat = "project-logs-{0:yyyy.MM.dd}"
                        })
                        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
                });
            }
        }
    }
}
