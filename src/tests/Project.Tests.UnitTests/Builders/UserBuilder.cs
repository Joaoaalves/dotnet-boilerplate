using Project.Domain.SharedKernel.Users;
using Project.Domain.Users;

namespace Project.Tests.UnitTests.Builders
{
    public class UserBuilder
    {

        private Name? FirstName;
        private Name? LastName;
        private Email? Email;
        private UserName? UserName;

        public UserBuilder WithName(Name firstName, Name lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            return this;
        }

        public UserBuilder WithEmail(Email email)
        {
            Email = email;
            return this;
        }

        public UserBuilder WithUserName(UserName userName)
        {
            UserName = userName;
            return this;
        }

        public static User WithDefaultValues()
        {
            var firstName = new Name("John");
            var lastName = new Name("Doe");
            var email = new Email("john@doe.com");
            var userName = new UserName("john@doe.com");

            return User.Create(firstName, lastName, userName, email);
        }

        public User Build()
        {
            if (FirstName is null || LastName is null || Email is null || UserName is null)
            {
                throw new Exception("You must set all values for User");
            }

            return User.Create(FirstName, LastName, UserName, Email);
        }
    }
}