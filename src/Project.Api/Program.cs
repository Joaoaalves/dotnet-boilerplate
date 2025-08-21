using Project.Api.Auth;
using Project.Api.Configurations;
using Project.Api.Extensions;
using Project.Domain.Users;
using Project.Infrastructure.Database;
using Project.Infrastructure.Logging;
using Project.Infrastructure.Monitoring;
using Project.Infrastructure.Processing;

var builder = WebApplication.CreateBuilder(args);
bool.TryParse(Environment.GetEnvironmentVariable("ENABLE_MONITORING"), out bool isMonitoringEnabled);
var configuration = builder.Configuration;
var env = builder.Environment;

var services = builder.Services;
var host = builder.Host;

// Setup logging (Serilog + Elasticsearch se habilitado via env)
host.SetupLoggingModule();

// Core services
services.AddCorsConfiguration();
services.AddSwaggerConfiguration();
services.AddValidations();
services.AddEndpointsApiExplorer();

services.AddControllers();

services.AddAuthModule();

services.AddConfigurations();
services.AddDataAccessModule();
services.AddMediatorModule();
services.AddMonitoringModule(isMonitoringEnabled);

// LoggingModule precisa do env
services.AddLogginModule(env);

var app = builder.Build();

if (env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoint Healthcheck
app.MapGet("/health", () => Results.Ok("Healthy"))
   .WithName("HealthCheck")
   .WithTags("Health");

app.UseCors("PublicPolicy");

app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
if (isMonitoringEnabled)
{
    app.UseOpenTelemetryPrometheusScrapingEndpoint();
}

app.MapIdentityApi<User>();

app.Run();

/// <summary>
///  Needed for tests purpose
/// </summary>
public partial class Program { }
