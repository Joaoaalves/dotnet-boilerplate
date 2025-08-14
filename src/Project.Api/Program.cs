using Project.Api.Configurations;
using Project.Api.Extensions;
using Project.API.Configurations;
using Project.Domain.Users;
using Project.Infrastructure.Database;
using Project.Infrastructure.Logging;
using Project.Infrastructure.Monitoring;
using Project.Infrastructure.Processing;


var builder = WebApplication.CreateBuilder(args);

var isProductionEnv = Environment.GetEnvironmentVariable("PRODUCTION") ?? "false";
var isDevelopment = isProductionEnv == "false";

var services = builder.Services;
var host = builder.Host;

host.SetupLoggingModule();

// Cors
services.AddCorsConfiguration();

services.AddSwaggerConfiguration();
services.AddEndpointsApiExplorer();

services.AddControllers();

services.AddConfigurations();
services.AddDataAccessModule();
services.AddMediatorModule();
services.AddMonitoringModule();

var app = builder.Build();

if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoint Healthcheck
app.MapGet("/health", () =>
{
    Results.Ok("Healthy");
})
   .WithName("HealthCheck")
   .WithTags("Health");

app.UseCors("PublicPolicy");

app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

var identityApi = app.MapIdentityApi<User>();

app.Run();