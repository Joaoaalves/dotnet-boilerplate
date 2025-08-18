namespace Project.Application.Configuration.Commands
{
    public interface ICommandPipelineBehaviour<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        Task<TResult> Handle(
            TCommand command,
            Func<TCommand, Task<TResult>> next,
            CancellationToken cancellationToken);
    }
}