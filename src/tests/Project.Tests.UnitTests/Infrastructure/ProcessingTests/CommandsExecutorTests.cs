using Microsoft.Extensions.DependencyInjection;
using Moq;
using Project.Application.Configuration.Commands;
using Project.Domain.SeedWork;
using Project.Infrastructure.Processing;

namespace Project.Tests.UnitTests.Infrastructure.ProcessingTests
{
    public class CommandsExecutorTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly ServiceCollection _services;

        public CommandsExecutorTests()
        {
            _services = new ServiceCollection();
            _services.AddScoped(_ => _mediatorMock.Object);
        }

        public record TestCommand(string Value) : ICommand<string> { public Guid Id => Guid.NewGuid(); }
        public record TestCommandNoResult(string Value) : ICommand<Unit>
        {
            public Guid Id => Guid.NewGuid();
        }

        [Fact]
        public async Task Execute_ShouldCallMediator_WhenNoBehaviors()
        {
            var sp = _services.BuildServiceProvider();
            var executor = new CommandsExecutor(sp);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ICommand<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("ok");

            var result = await executor.Execute(new TestCommand("123"));

            Assert.Equal("ok", result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ICommand<string>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldInvokeBehavior_BeforeMediator()
        {
            var behaviorMock = new Mock<ICommandPipelineBehaviour<TestCommand, string>>();
            behaviorMock
                .Setup(b => b.Handle(It.IsAny<TestCommand>(), It.IsAny<Func<TestCommand, Task<string>>>(), It.IsAny<CancellationToken>()))
                .Returns<TestCommand, Func<TestCommand, Task<string>>, CancellationToken>(
                    async (cmd, next, _) =>
                    {
                        var inner = await next(cmd);
                        return inner + "-wrapped";
                    });

            _services.AddScoped(_ => behaviorMock.Object);
            var sp = _services.BuildServiceProvider();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ICommand<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("base");

            var executor = new CommandsExecutor(sp);
            var result = await executor.Execute(new TestCommand("abc"));

            Assert.Equal("base-wrapped", result);
            behaviorMock.Verify(b => b.Handle(It.IsAny<TestCommand>(), It.IsAny<Func<TestCommand, Task<string>>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Execute_NoResultCommand_ShouldCallMediator()
        {
            var sp = _services.BuildServiceProvider();
            var executor = new CommandsExecutor(sp);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ICommand<Unit>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            await executor.Execute(new TestCommandNoResult("abc"));

            _mediatorMock.Verify(
                m => m.Send(It.IsAny<ICommand<Unit>>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}
