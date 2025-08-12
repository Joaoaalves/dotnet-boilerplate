using Project.Domain.SeedWork;

namespace Project.Application.Configuration.Queries
{
    public interface IQuery<TResult> : IRequest<TResult> { }
}