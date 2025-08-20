using Microsoft.Extensions.DependencyInjection;
using Project.Domain.SeedWork;

namespace Project.Infrastructure.Processing
{
    /// <summary>
    /// Implements a custom mediator to handle requests and notifications using runtime reflection.
    /// </summary>
    public class Mediator(IServiceProvider provider) : IMediator
    {
        private readonly IServiceProvider _provider = provider;

        /// <inheritdoc />
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handler = _provider.GetService(handlerType);

            var handleMethod = handlerType.GetMethod("Handle");

            var parameters = new object[] { request, cancellationToken };

            var behaviors = _provider
                .GetServices(typeof(IRequestPipelineBehavior<,>).MakeGenericType(request.GetType(), typeof(TResponse)))
                .Cast<dynamic>()
                .Aggregate(
                    () => (Task<TResponse>)handleMethod!.Invoke(handler, parameters)!,
                    (next, behavior) => () => behavior.Handle((dynamic)request, next, cancellationToken)
                );

            return await behaviors();
        }

        /// <inheritdoc />
        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());

            var handlers = _provider.GetServices(handlerType) ?? [];

            var handleMethod = handlerType.GetMethod("Handle");

            foreach (var handler in handlers)
            {
                var task = handleMethod!.Invoke(handler, [notification, cancellationToken]) as Task
                    ?? throw new InvalidOperationException($"Handler {handler!.GetType().Name} did not return a valid Task.");

                await task;
            }
        }
    }
}
