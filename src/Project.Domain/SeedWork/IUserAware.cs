using Project.Domain.Users;

namespace Project.Domain.SeedWork
{
    public interface IUserAware
    {
        void InjectUser(User user);
    }

}