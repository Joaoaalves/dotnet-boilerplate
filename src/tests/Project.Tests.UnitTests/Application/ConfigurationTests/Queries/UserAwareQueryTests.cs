using Project.Application.Configuration.Commands;
using Project.Application.Configuration.Queries;
using Project.Tests.UnitTests.Builders;

namespace Project.Tests.UnitTests.Application.ConfigurationTests.Queries
{
    public class UserAwareQueryTests
    {
        // Concrete class
        private class TestUserAwareQuery : UserAwareQuery<Unit> { }

        [Fact]
        public void InjectUser_ShouldSetUserProperty()
        {
            // Arrange
            var query = new TestUserAwareQuery();
            var user = UserBuilder.WithDefaultValues();

            // Act
            query.InjectUser(user);

            // Assert
            Assert.NotNull(query);
            Assert.NotNull(query.User);
            Assert.Equal(user, query.User);
            Assert.True(user.Equals(query.User));
        }
    }
}