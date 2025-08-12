using Project.Domain.SeedWork;
using Project.Domain.Users;

namespace Project.Application.Configuration.Queries
{
    public class UserAwareQuery<TResult> : IUserAware, IQuery<TResult>
    {
        public User User { get; private set; } = default!;

        public void InjectUser(User user)
        {
            User = user;
        }
    }
}