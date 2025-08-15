using Project.Application.Configuration.Commands;
using Project.Domain.SeedWork;
using Microsoft.Extensions.DependencyInjection;

namespace Project.Infrastructure.Processing
{
    /// <summary>
    /// Pipeline behavior that ensures Unit of Work is committed if the command succeeds, or reverted if an exception occurs.
    /// </summary>
    /// <typeparam name="TRequest">The command type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public class UnitOfWorkPipelineBehavior<TRequest, TResponse>(IServiceProvider serviceProvider) : IRequestPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken)
        {
            var uow = _serviceProvider.GetRequiredService<IUnitOfWork>();
            try
            {
                var result = await next();
                await uow.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await uow.RevertAsync();
                throw;
            }
        }
    }
}
