using Microsoft.AspNetCore.Http;

using Project.Domain.SeedWork;

using Project.Application.Users;
using Project.Application.Configuration.Commands;

namespace Project.Application.Configuration.Commands.Behaviors
{
    public class UserInjectionCommandBehavior<TCommand, TResult>(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository)
        : ICommandPipelineBehaviour<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<TResult> Handle(
            TCommand command,
            Func<TCommand, Task<TResult>> next,
            CancellationToken cancellationToken
        )
        {
            if (command is IUserAware userAware)
            {
                var claimsPrincipal = _httpContextAccessor.HttpContext?.User
                    ?? throw new Exception("User not found in context.");

                var userId = claimsPrincipal?.Identity?.Name
                    ?? throw new Exception("Failed to resolve authenticated user.");

                var user = await _userRepository.GetByEmailAsync(userId) ?? throw new Exception("Failed to resolve authenticated user from repository.");

                userAware.InjectUser(user);
            }

            return await next(command);
        }


    }
}
