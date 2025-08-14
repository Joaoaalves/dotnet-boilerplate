using Project.Domain.SeedWork;
using Project.Domain.Users;

namespace Project.Application.Configuration.Commands
{
    public abstract class UserAwareCommand<TResult> : IUserAware, ICommand<TResult>
    {
        public Guid Id => Guid.NewGuid();
        public User User { get; private set; } = default!;

        public void InjectUser(User user)
        {
            User = user;
        }
    }
}