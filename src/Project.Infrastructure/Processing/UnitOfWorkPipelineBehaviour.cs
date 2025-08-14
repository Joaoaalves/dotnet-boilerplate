using Project.Application.Configuration.Commands;
using Project.Domain.SeedWork;
using Microsoft.Extensions.DependencyInjection;

namespace Project.Infrastructure.Processing
{
    public class UnitOfWorkPipelineBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkPipelineBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

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