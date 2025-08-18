using Microsoft.Extensions.DependencyInjection;
using Project.Application.Configuration.Commands;
using Project.Application.Configuration.Commands.Behaviors;
using Project.Domain.SeedWork;

namespace Project.Infrastructure.Processing
{
    /// <summary>
    /// Executes application commands, resolving the required handlers and behaviors via dependency injection.
    /// </summary>
    public class CommandsExecutor(IServiceProvider provider)
    {
        private readonly IServiceProvider _provider = provider;

        /// <summary>
        /// Executes a command and returns a response using pipeline behaviors and command handlers.
        /// </summary>
        /// <typeparam name="TResult">The result type of the command.</typeparam>
        /// <param name="command">The command to be executed.</param>
        /// <returns>The command result.</returns>
        public async Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            using var scope = _provider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var behaviorsType = typeof(ICommandPipelineBehaviour<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var behaviors = scope.ServiceProvider.GetServices(behaviorsType)
                .Cast<dynamic>()
                .Reverse()
                .ToList();

            Task<TResult> handlerDelegate(dynamic cmd) => mediator.Send((ICommand<TResult>)cmd);

            var pipeline = behaviors.Aggregate(
                handlerDelegate,
                (next, behavior) => (cmd) => behavior.Handle(cmd, next, CancellationToken.None)
            );

            return await pipeline(command);
        }

        /// <summary>
        /// Executes a command that does not return a result.
        /// </summary>
        /// <param name="command">The command to be executed.</param>
        public async Task Execute(ICommand command)
        {
            using var scope = _provider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(command);
        }
    }
}
