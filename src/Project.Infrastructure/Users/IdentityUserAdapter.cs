using Microsoft.AspNetCore.Identity;
using Project.Domain.Users;
using Project.Domain.SharedKernel.Users;

namespace Project.Infrastructure.Users
{
    public class IdentityUserAdapter : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public static IdentityUserAdapter FromDomain(User user)
        {
            return new IdentityUserAdapter
            {
                Id = user.Id.Value.ToString(),
                Email = user.Email.Value,
                UserName = user.UserName.Value,
                FirstName = user.FirstName.Value,
                LastName = user.LastName.Value,
            };
        }

        public User ToDomain()
        {
            return User.Create(
                new Name(FirstName),
                new Name(LastName),
                new UserName(UserName!),
                new Email(Email!)
            );
        }
    }
}