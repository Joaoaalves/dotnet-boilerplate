using Project.Domain.SeedWork;

namespace Project.Tests.UnitTests.Infrastructure.Fakes
{

    namespace Project.Tests.UnitTests.Infrastructure.ProcessingTests
    {
        public class FakeMediator : IMediator
        {
            public readonly List<INotification> PublishedEvents = new();

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException("Not needed for these tests");
            }

            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
                where TNotification : INotification
            {
                PublishedEvents.Add(notification);
                return Task.CompletedTask;
            }
        }
    }

}