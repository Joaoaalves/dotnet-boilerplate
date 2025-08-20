using Project.Domain.SeedWork;
using Project.Domain.Users;

namespace Project.Tests.UnitTests.Application.Fakes
{
    public class FakeUserAwareQuery<TResult> : IRequest<TResult>, IUserAware
    {
        public User? InjectedUser { get; private set; }

        public void InjectUser(User user)
        {
            InjectedUser = user;
        }
    }
}