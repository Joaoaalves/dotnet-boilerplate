using Microsoft.Extensions.DependencyInjection;
using Project.Infrastructure.Database;
using Project.Tests.Integration.Setup;

namespace Project.Tests.Integration;

/// <summary>
/// Base class for integration tests. 
/// It handles database setup, HttpClient initialization, and scoped services.
/// </summary>
public class IntegrationTestBase : IClassFixture<SharedDatabaseFixture>, IAsyncLifetime
{
    protected readonly SharedDatabaseFixture DbFixture;
    protected readonly HttpClient Client;
    protected readonly ApplicationDbContext DbContext;
    private readonly IServiceScope _scope;

    public IntegrationTestBase(SharedDatabaseFixture dbFixture)
    {
        DbFixture = dbFixture;

        var factory = new CustomWebApplicationFactory();
        factory.SetConnectionString(DbFixture.ConnectionString);
        Client = factory.CreateClient();

        _scope = factory.Services.CreateScope();
        DbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    public async Task InitializeAsync() => await DbFixture.ResetDbAsync();

    public Task DisposeAsync()
    {
        _scope.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Adds a fake Authorization header for the given user email.
    /// This triggers the TestAuthHandler in integration tests.
    /// </summary>
    protected void AuthenticateClient(string email)
    {
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", email);
    }
}
