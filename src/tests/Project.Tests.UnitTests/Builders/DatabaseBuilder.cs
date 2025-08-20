using Microsoft.EntityFrameworkCore;
using Project.Domain.Users;
using Project.Tests.UnitTests.Infrastructure.Fakes;

namespace Project.Tests.UnitTests.Builders
{
    public class DatabaseBuilder
    {
        private static readonly DbContextOptions<FakeDbContext<User>> _options =
            new DbContextOptionsBuilder<FakeDbContext<User>>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        public static FakeDbContext<User> InMemoryDatabase()
        {
            return new FakeDbContext<User>(_options);
        }
    }
}