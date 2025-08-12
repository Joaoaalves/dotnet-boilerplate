using Project.Domain.SeedWork;
using Project.Domain.SharedKernel.Users;

namespace Project.Domain.Users
{
    /// <summary>
    /// Represents a domain user.
    /// </summary>
    public class User : IAggregateRoot
    {
        /// <summary>
        /// Gets the unique identifier for the user.
        /// </summary>
        public UserId Id { get; private set; } = null!;

        /// <summary>
        /// Gets the user's email address.
        /// </summary>
        public Email Email { get; private set; } = null!;

        /// <summary>
        /// Gets the user's username.
        /// </summary>
        public UserName UserName { get; private set; } = null!;

        /// <summary>
        /// Gets the user's first name.
        /// </summary>
        public Name FirstName { get; private set; } = null!;

        /// <summary>
        /// Gets the user's last name.
        /// </summary>
        public Name LastName { get; private set; } = null!;

        [Obsolete("Only for EF", true)]
        private User() { }

        private User(UserId id, Name firstName, Name lastName, UserName userName, Email email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = userName;
        }

        /// <summary>
        /// Creates a new <see cref="User"/> instance.
        /// </summary>
        public static User Create(Name firstName, Name lastName, UserName userName, Email email)
        {
            return new User(UserId.NewId(), firstName, lastName, userName, email);
        }
    }
}
