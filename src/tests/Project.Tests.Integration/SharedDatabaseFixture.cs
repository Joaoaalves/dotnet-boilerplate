using Microsoft.EntityFrameworkCore;
using Npgsql;
using Project.Infrastructure.Database;
using Respawn;
using Testcontainers.PostgreSql;

namespace Project.Tests.Integration
{
    public class SharedDatabaseFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer;
        private Respawner? _respawner;
        public string ConnectionString => _dbContainer.GetConnectionString();

        public SharedDatabaseFixture()
        {
            _dbContainer = new PostgreSqlBuilder()
                .WithDatabase("test_db")
                .WithUsername("test_user")
                .WithPassword("test_password")
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            await ApplyMigrations();

            await using var conn = new NpgsqlConnection(ConnectionString);

            await conn.OpenAsync();

            _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"]
            });
        }

        public async Task ResetDbAsync()
        {
            await using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            if (_respawner is not null)
                await _respawner.ResetAsync(conn);
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }

        private async Task ApplyMigrations()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

            using var dbContext = new ApplicationDbContext(options);
            await dbContext.Database.MigrateAsync();
        }
    }
}