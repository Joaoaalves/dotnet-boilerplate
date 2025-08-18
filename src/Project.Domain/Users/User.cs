using Microsoft.AspNetCore.Identity;
using Project.Domain.SeedWork;
using Project.Domain.SharedKernel.Users;

namespace Project.Domain.Users
{
    /// <summary>
    /// Represents the application user, inheriting from IdentityUser to use ASP.NET Core Identity features directly.
    /// </summary>
    public class User : IdentityUser, IAggregateRoot
    {
        public Name FirstName { get; private set; } = null!;
        public Name LastName { get; private set; } = null!;


        // Required by EF Core
        [Obsolete("Only for EF/Identity serialization", true)]
        public User() { }

        private User(string userName, string email, Name firstName, Name lastName)
        {
            UserName = userName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        /// <summary>
        /// Factory method to create a new User instance.
        /// </summary>
        public static User Create(Name firstName, Name lastName, UserName userName, Email email)
        {

            return new User(userName.Value, email.Value, firstName, lastName);
        }

        public void Rename(Name? firstName, Name? lastName)
        {
            if (firstName is not null)
                FirstName = firstName;

            if (lastName is not null)
                LastName = lastName;
        }
    }
}
