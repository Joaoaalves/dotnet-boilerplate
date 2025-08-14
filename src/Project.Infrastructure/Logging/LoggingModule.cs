using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.Elasticsearch;

namespace Project.Infrastructure.Logging
{
    public static class LoggingModule
    {
        public static void SetupLoggingModule(this IHostBuilder hostBuilder)
        {
            var isLogginEnabled = Environment.GetEnvironmentVariable("ENABLE_LOGGING");
            if (isLogginEnabled is null)
                return;

            if (isLogginEnabled == "true")
            {
                var elasticUri = Environment.GetEnvironmentVariable("ELASTICSEARCH_URL") ?? throw new Exception("You must provide ELASTICSEARCH_URL param, otherwise you should disable Logging.");

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