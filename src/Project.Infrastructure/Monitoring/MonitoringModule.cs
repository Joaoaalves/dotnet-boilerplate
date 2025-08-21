using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Project.Infrastructure.Monitoring
{
    /// <summary>
    /// Provides extension methods to register monitoring and observability features using OpenTelemetry.
    /// </summary>
    public static class MonitoringModule
    {
        /// <summary>
        /// Registers OpenTelemetry metrics exporters and instrumentation if monitoring is enabled.
        /// </summary>
        /// <param name="services">The service collection used to register services.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        /// <remarks>
        /// Monitoring is conditionally enabled via the ENABLE_MONITORING environment variable.
        /// Metrics include ASP.NET Core, runtime, and process metrics with a Prometheus exporter.
        /// </remarks>
        public static IServiceCollection AddMonitoringModule(this IServiceCollection services, bool isMonitoringEnabled)
        {

            if (isMonitoringEnabled)
            {
                var serviceName = Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "Project.Api";

                services.AddOpenTelemetry()
                    .WithMetrics(metrics =>
                        metrics
                            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                            .AddMeter("*")
                            .AddAspNetCoreInstrumentation()
                            .AddRuntimeInstrumentation()
                            .AddProcessInstrumentation()
                            .AddPrometheusExporter()
                    );
            }

            return services;
        }
    }
}
