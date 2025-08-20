using Project.Application.Configuration.Commands;

namespace Project.Tests.UnitTests.Application.ConfigurationTests.Commands
{
    public class InternalCommandBaseTests
    {
        // Concrete class
        private class TestCommand : InternalCommandBase<Unit>
        {
            public TestCommand() : base() { }
            public TestCommand(Guid id) : base(id) { }
        }

        [Fact]
        public void Constructor_ShouldGenerateNewId_WhenNoIdProvided()
        {
            // Arrange & Act
            var command = new TestCommand();

            // Assert
            Assert.NotEqual(Guid.Empty, command.Id);
        }

        [Fact]
        public void Constructor_ShouldUseProvidedId_WhenIdProvided()
        {
            // Arrange
            var expectedId = Guid.NewGuid();

            // Act
            var command = new TestCommand(expectedId);

            // Assert
            Assert.Equal(expectedId, command.Id);
        }
    }
}