using Microsoft.Extensions.DependencyInjection;
using Project.Application.Configuration.Queries;
using Project.Domain.SeedWork;

namespace Project.Infrastructure.Processing
{
    /// <summary>
    /// Executes queries using the registered pipeline behaviors and query handlers.
    /// </summary>
    public class QueriesExecutor(IServiceProvider provider)
    {
        private readonly IServiceProvider _provider = provider;

        /// <summary>
        /// Executes a query using any applicable pipeline behaviors and returns a result.
        /// </summary>
        /// <typeparam name="TResult">The type of result expected from the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <returns>A task that represents the asynchronous operation, returning the query result.</returns>
        public async Task<TResult> Execute<TResult>(IQuery<TResult> query)
        {
            using var scope = _provider.CreateScope();
            var sp = scope.ServiceProvider;

            var mediator = sp.GetRequiredService<IMediator>();

            var behaviors = sp
                .GetServices(typeof(IRequestPipelineBehavior<,>).MakeGenericType(query.GetType(), typeof(TResult)))
                .Cast<dynamic>()
                .Reverse()
                .ToList();

            Func<IQuery<TResult>, Task<TResult>> handler = (q) => mediator.Send(q);

            foreach (var behavior in behaviors)
            {
                var next = handler;
                handler = (q) =>
                {
                    Task<TResult> nextDelegate() => next(q);
                    return behavior.Handle((dynamic)q, (Func<Task<TResult>>)nextDelegate, CancellationToken.None);
                };
            }

            return await handler(query);
        }
    }
}
