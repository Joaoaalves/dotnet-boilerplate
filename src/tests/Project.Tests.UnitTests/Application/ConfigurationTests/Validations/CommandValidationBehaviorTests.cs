using FluentValidation;
using Project.Application.Configuration.Commands;
using Project.Application.Configuration.Validation;
using Project.Tests.UnitTests.Application.Fakes;


namespace Project.Tests.UnitTests.Application.ConfigurationTests.Validations
{
    public class CommandValidationBehaviorTests
    {
        [Fact]
        public async Task Handle_ShouldCallNext_WhenNoValidators()
        {
            var behavior = new CommandValidationBehavior<FakeCommand, Unit>(new List<IValidator<FakeCommand>>());
            var command = new FakeCommand { Name = "test" };

            var nextCalled = false;
            Task<Unit> Next()
            {
                nextCalled = true;
                return Task.FromResult(Unit.Value);
            }

            var result = await behavior.Handle(command, Next, CancellationToken.None);

            Assert.True(nextCalled);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ShouldCallNext_WhenValidationPasses()
        {
            var validators = new List<IValidator<FakeCommand>> { new FakeCommandValidator() };
            var behavior = new CommandValidationBehavior<FakeCommand, Unit>(validators);
            var command = new FakeCommand { Name = "Valid" };

            var result = await behavior.Handle(command, () => Task.FromResult(Unit.Value), CancellationToken.None);

            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidCommandException_WhenValidationFails()
        {
            var validators = new List<IValidator<FakeCommand>> { new FakeCommandValidator() };
            var behavior = new CommandValidationBehavior<FakeCommand, Unit>(validators);
            var command = new FakeCommand { Name = "" }; // inválido

            var ex = await Assert.ThrowsAsync<InvalidCommandException>(() =>
                behavior.Handle(command, () => Task.FromResult(Unit.Value), CancellationToken.None)
            );

            Assert.Equal("Name is required", ex.Message);
            Assert.Equal("Name is required", ex.Details);
        }
    }
}
