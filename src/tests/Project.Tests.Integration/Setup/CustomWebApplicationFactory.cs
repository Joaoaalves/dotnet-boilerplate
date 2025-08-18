using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Project.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Project.Tests.Integration.Setup
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public string ConnectionString { get; private set; } = string.Empty;
        public CustomWebApplicationFactory()
        {
            EnvLoader.EnsureLoaded();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Set Testing Environment
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.AddJsonFile("appsettings.json", optional: true);
                configBuilder.AddJsonFile("appsettings.Test.json", optional: true);
            });

            builder.ConfigureServices(services =>
            {
                // Replace real authentication with the test scheme
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.TestScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.TestScheme;
                }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.TestScheme, _ => { });
            });

            builder.ConfigureServices(services =>
            {
                // Remove default DbContext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)
                );
                if (descriptor is not null)
                    services.Remove(descriptor);

                // Add DbContext with container connection string
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(ConnectionString));
            });
        }

        public void SetConnectionString(string connString) => ConnectionString = connString;
    }
}