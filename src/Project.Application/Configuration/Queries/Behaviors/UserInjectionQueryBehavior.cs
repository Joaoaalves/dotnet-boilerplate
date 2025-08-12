using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Project.Domain.SeedWork;
using Project.Domain.Users;

namespace Project.Application.Configuration.Queries.Behaviors
{
    public class UserInjectionQueryBehavior<TQuery, TResult>(
        IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager)
        : IRequestPipelineBehavior<TQuery, TResult>
        where TQuery : IRequest<TResult>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<TResult> Handle(
            TQuery query,
            Func<Task<TResult>> next,
            CancellationToken cancellationToken)
        {
            if (query is IUserAware userAware)
            {
                var claimsPrincipal = _httpContextAccessor.HttpContext?.User
                    ?? throw new Exception("User not found in context.");

                var user = await _userManager.GetUserAsync(claimsPrincipal)
                    ?? throw new Exception("Failed to resolve authenticated user.");

                userAware.InjectUser(user);
            }

            return await next();
        }
    }
}
