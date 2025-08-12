using Project.Api.Extensions;
using Project.API.Configurations;
using Project.Infrastructure.Database;
using Project.Infrastructure.Processing;
using Project.Infrastructure.Users;

var builder = WebApplication.CreateBuilder(args);

var isProductionEnv = Environment.GetEnvironmentVariable("PRODUCTION") ?? "false";
var isDevelopment = isProductionEnv == "false";

var services = builder.Services;

// Cors
services.AddCorsConfiguration();

services.AddSwaggerConfiguration();
services.AddEndpointsApiExplorer();

services.AddControllers();

services.AddDataAccessModule();
services.AddMediatorModule();

var app = builder.Build();

if (isDevelopment)
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

var identityApi = app.MapIdentityApi<IdentityUserAdapter>();

app.Run();