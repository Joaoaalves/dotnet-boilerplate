using Project.Application.Users.Commands.RegisterUser;

namespace Project.Tests.UnitTests.Builders
{
    public class RegisterUserCommandBuilder
    {
        private string? FirstName;
        private string? LastName;
        private string? Email;
        private string? Password;

        public RegisterUserCommandBuilder WithName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            return this;
        }

        public RegisterUserCommandBuilder WithEmail(string email)
        {
            Email = email;
            return this;
        }

        public RegisterUserCommandBuilder WithPassword(string password)
        {
            Password = password;
            return this;
        }

        public static RegisterUserCommand WithDefaultValue()
        {
            var firstName = "John";
            var lastName = "Doe";
            var email = "john@doe.com";
            var password = "Str0ngP@ss123";

            return new RegisterUserCommand(firstName, lastName, email, password);
        }

        public RegisterUserCommand Build()
        {
            if (FirstName is null || LastName is null || Email is null || Password is null)
            {
                throw new Exception("You must set all values for RegisterUserCommand");
            }


            return new RegisterUserCommand(FirstName, LastName, Email, Password);
        }
    }
}