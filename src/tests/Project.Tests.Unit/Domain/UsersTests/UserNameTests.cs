using FluentAssertions;
using Project.Domain.SeedWork;
using Project.Domain.SharedKernel.Users;
using Project.Domain.SharedKernel.Users.Rules;

namespace Project.Tests.Unit.Domain.UsersTests
{
    public class UserNameTests
    {
        [Theory]
        [InlineData("john_doe@email.com")]
        [InlineData("user123@email.com")]
        [InlineData("ÉricDupont@email.com")]
        public void ShouldCreateWithValidValue(string userNameStr)
        {
            var userName = new UserName(userNameStr);

            userName.Should().NotBeNull();
            userName.Value.Should().Be(userNameStr);
        }

        [Theory]
        [InlineData("1@c.1")]
        [InlineData("3")]           // too short
        [InlineData("invalid!name")] // invalid char
        public void ShouldNotCreateWithInvalidValue(string userNameStr)
        {
            var message = new UserNameMustBeValidRule(userNameStr).Message;

            Action act = () => new UserName(userNameStr);

            act.Should()
               .Throw<BusinessRuleValidationException>()
               .WithMessage(message);
        }
    }
}