using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Project.Application.Users;
using Project.Domain.SeedWork;
using Project.Domain.Users;

namespace Project.Application.Configuration.Queries.Behaviors
{
    public class UserInjectionQueryBehavior<TQuery, TResult>(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository)
        : IRequestPipelineBehavior<TQuery, TResult>
        where TQuery : IRequest<TResult>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<TResult> Handle(
            TQuery query,
            Func<Task<TResult>> next,
            CancellationToken cancellationToken)
        {
            if (query is IUserAware userAware)
            {
                var claimsPrincipal = _httpContextAccessor.HttpContext?.User
                    ?? throw new Exception("User not found in context.");

                var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email) ?? throw new Exception("Email not found on Claims");

                var user = await _userRepository.GetByEmailAsync(email)
                    ?? throw new Exception("Failed to resolve authenticated user.");

                userAware.InjectUser(user);
            }

            return await next();
        }
    }
}
