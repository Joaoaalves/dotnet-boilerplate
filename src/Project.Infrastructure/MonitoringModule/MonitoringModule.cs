using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Project.Infrastructure.Monitoring
{
    public static class MonitoringModule
    {
        public static IServiceCollection AddMonitoringModule(this IServiceCollection services)
        {
            bool.TryParse(Environment.GetEnvironmentVariable("ENABLE_MONITORING"), out bool isMonitoringEnabled);

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