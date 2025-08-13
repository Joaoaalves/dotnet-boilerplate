using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Project.Infrastructure.Observability
{
    public static class ObservabilityModule
    {
        public static IServiceCollection AddObservabilityModule(this IServiceCollection services)
        {
            services.AddOpenTelemetry()
                .WithMetrics(metrics =>
                    metrics
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Project.Api"))
                        .AddMeter("*")
                        .AddAspNetCoreInstrumentation()
                        .AddRuntimeInstrumentation()
                        .AddProcessInstrumentation()
                        .AddPrometheusExporter()
                );
            return services;
        }
    }
}