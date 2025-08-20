using Microsoft.Extensions.DependencyInjection;
using Moq;
using Project.Application.Configuration.Commands;
using Project.Application.Configuration.Queries;
using Project.Domain.SeedWork;
using Project.Infrastructure.Processing;
using Project.Tests.UnitTests.Infrastructure.Fakes;

namespace Project.Tests.UnitTests.Infrastructure.ProcessingTests
{

    public class MediatorTests
    {
        private readonly ServiceCollection _services;
        private static readonly string[] expected = ["2-start", "1-start", "1-end", "2-end"];

        private ServiceProvider BuildProvider() => _services.BuildServiceProvider();
        public MediatorTests()
        {
            _services = new ServiceCollection();
        }

        [Fact]
        public async Task Send_ShouldInvokeHandlerAndReturnResponse()
        {
            // Arrange
            var request = new FakeRequest();
            var handlerMock = new Mock<IRequestHandler<FakeRequest, string>>();
            handlerMock.Setup(h => h.Handle(request, It.IsAny<CancellationToken>()))
                       .ReturnsAsync("success");

            _services.AddSingleton(handlerMock.Object);

            var mediator = new Mediator(BuildProvider());

            // Act
            var result = await mediator.Send(request);

            // Assert
            Assert.Equal("success", result);
            handlerMock.Verify(h => h.Handle(request, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Send_ShouldExecutePipelineBehaviorsInOrder()
        {
            // Arrange
            var request = new TestQuery();
            var executionOrder = new List<string>();

            var behavior1 = new Mock<IRequestPipelineBehavior<TestQuery, string>>();
            behavior1.Setup(b => b.Handle(request, It.IsAny<Func<Task<string>>>(), It.IsAny<CancellationToken>()))
                     .Returns(async (TestQuery req, Func<Task<string>> next, CancellationToken ct) =>
                     {
                         executionOrder.Add("1-start");
                         var result = await next();
                         executionOrder.Add("1-end");
                         return result;
                     });

            var behavior2 = new Mock<IRequestPipelineBehavior<TestQuery, string>>();
            behavior2.Setup(b => b.Handle(request, It.IsAny<Func<Task<string>>>(), It.IsAny<CancellationToken>()))
                     .Returns(async (TestQuery req, Func<Task<string>> next, CancellationToken ct) =>
                     {
                         executionOrder.Add("2-start");
                         var result = await next();
                         executionOrder.Add("2-end");
                         return result;
                     });

            var handler = new Mock<IRequestHandler<TestQuery, string>>();
            handler.Setup(h => h.Handle(request, It.IsAny<CancellationToken>()))
                   .ReturnsAsync("final");

            _services.AddSingleton(handler.Object);
            _services.AddSingleton(behavior1.Object);
            _services.AddSingleton(behavior2.Object);

            var mediator = new Mediator(BuildProvider());

            // Act
            var result = await mediator.Send<string>(request);

            // Assert
            Assert.Equal("final", result);
            Assert.Equal(expected, executionOrder);
        }

        public class TestQuery : IQuery<string> { }

        private class TestNotification : INotification { }

        private class InvalidNotificationHandler : INotificationHandler<TestNotification>
        {
            Task INotificationHandler<TestNotification>.Handle(TestNotification notification, CancellationToken cancellationToken) => Task.FromResult("not a task");
        }
    }
}