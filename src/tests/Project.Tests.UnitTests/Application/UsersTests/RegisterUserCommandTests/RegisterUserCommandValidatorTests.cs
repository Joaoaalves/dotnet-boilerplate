using FluentValidation.Results;
using Project.Application.Users.Commands.RegisterUser;
using Project.Tests.UnitTests.Builders;

namespace Project.Tests.UnitTests.Application.UsersTests.RegisterUserCommandTests
{
    public class RegisterUserCommandValidatorTests
    {
        private readonly RegisterUserCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_When_AllFieldsAreValid()
        {
            var command = RegisterUserCommandBuilder.WithDefaultValue();

            ValidationResult result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Fail_When_FirstName_IsEmpty()
        {
            var command = new RegisterUserCommandBuilder().WithName("", "Doe").WithEmail("john.doe@example.com").WithPassword("Valid1!").Build();

            ValidationResult result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "FirstName");
        }

        [Fact]
        public void Should_Fail_When_LastName_IsEmpty()
        {
            var command = new RegisterUserCommandBuilder().WithName("John", "").WithEmail("john.doe@example.com").WithPassword("Valid1!").Build();


            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "LastName");
        }

        [Fact]
        public void Should_Fail_When_Email_IsInvalid()
        {
            var command = new RegisterUserCommandBuilder().WithName("John", "Doe").WithEmail("not-an-email").WithPassword("Valid1!").Build();


            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Email");
        }

        [Fact]
        public void Should_Fail_When_Password_TooShort()
        {
            var command = new RegisterUserCommandBuilder().WithName("John", "Doe").WithEmail("john.doe@example.com").WithPassword("weak").Build();


            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Password");
        }

        [Theory]
        [InlineData("lowercase1!")]   // no uppercase
        [InlineData("UPPERCASE1!")]   // no lowercase
        [InlineData("NoNumber!")]     // no digit
        [InlineData("NoSpecial1")]    // no special char
        public void Should_Fail_When_Password_MissingRequirement(string password)
        {
            var command = new RegisterUserCommandBuilder().WithName("John", "Doe").WithEmail("john.doe@example.com").WithPassword(password).Build();

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Password");
        }
    }
}
