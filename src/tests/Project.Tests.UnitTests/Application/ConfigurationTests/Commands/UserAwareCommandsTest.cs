using Project.Application.Configuration.Commands;
using Project.Tests.UnitTests.Builders;

namespace Project.Tests.UnitTests.Application.ConfigurationTests.Commands
{
    public class UserAwareCommandsTest
    {
        // Concrete class
        private class TestUserAwareCommand : UserAwareCommand<Unit>
        {
        }

        [Fact]
        public void Id_ShouldBeGeneratedAutomatically()
        {
            // Arrange & Act
            var command = new TestUserAwareCommand();

            // Assert
            Assert.NotNull(command);
            Assert.NotEqual(Guid.Empty, command.Id);
        }

        [Fact]
        public void InjectUser_ShouldSetUserProperty()
        {
            // Arrange
            var command = new TestUserAwareCommand();
            var user = UserBuilder.WithDefaultValues();

            // Act
            command.InjectUser(user);

            // Assert
            Assert.NotNull(command.User);
            Assert.Equal(user, command.User);
            Assert.True(user.Equals(command.User));
        }
    }
}