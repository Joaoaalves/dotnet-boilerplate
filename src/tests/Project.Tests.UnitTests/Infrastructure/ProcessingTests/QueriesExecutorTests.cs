using Microsoft.Extensions.DependencyInjection;
using Moq;
using Project.Application.Configuration.Queries;
using Project.Domain.SeedWork;
using Project.Infrastructure.Processing;

namespace Project.Tests.UnitTests.Infrastructure.ProcessingTests
{
    public class QueriesExecutorTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly ServiceCollection _services;

        public QueriesExecutorTests()
        {
            _services = new ServiceCollection();
            _services.AddScoped(_ => _mediatorMock.Object);
        }

        public record TestQuery(string Value) : IQuery<string>;

        [Fact]
        public async Task Execute_ShouldCallMediator_WhenNoBehaviors()
        {
            var sp = _services.BuildServiceProvider();
            var executor = new QueriesExecutor(sp);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<IQuery<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("result");

            var result = await executor.Execute(new TestQuery("q1"));

            Assert.Equal("result", result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IQuery<string>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldInvokeBehavior_BeforeMediator()
        {
            var behaviorMock = new Mock<IRequestPipelineBehavior<TestQuery, string>>();
            behaviorMock
                .Setup(b => b.Handle(It.IsAny<TestQuery>(), It.IsAny<Func<Task<string>>>(), It.IsAny<CancellationToken>()))
                .Returns<TestQuery, Func<Task<string>>, CancellationToken>(
                    async (q, next, _) =>
                    {
                        var inner = await next();
                        return inner + "-viaBehavior";
                    });

            _services.AddScoped(_ => behaviorMock.Object);
            var sp = _services.BuildServiceProvider();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<IQuery<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("base");

            var executor = new QueriesExecutor(sp);
            var result = await executor.Execute(new TestQuery("q1"));

            Assert.Equal("base-viaBehavior", result);
            behaviorMock.Verify(b => b.Handle(It.IsAny<TestQuery>(), It.IsAny<Func<Task<string>>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}