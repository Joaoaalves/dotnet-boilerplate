using FluentAssertions;
using Project.Domain.SeedWork;
using Project.Domain.SharedKernel.Users;
using Project.Domain.SharedKernel.Users.Rules;

namespace Project.Tests.Unit.Domain.UsersTests
{
    public class EmailTests
    {
        [Theory]
        [InlineData("john.doe@example.com")]
        [InlineData("user+alias@gmail.com")]
        [InlineData("Éric@example.fr")] // EC - Special Char
        [InlineData("john@mail.example.co.uk")] // EC - Subdomain
        public void ShouldCreateWithValidValue(string emailStr)
        {
            var email = new Email(emailStr);

            email.Should().NotBeNull();
            email.Value.Should().Be(emailStr);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalid-email")]
        [InlineData("user@com")]
        public void ShouldNotCreateWithInvalidValue(string emailStr)
        {
            var message = new EmailMustBeValidRule(emailStr).Message;

            Action act = () => new Email(emailStr);

            act.Should()
               .Throw<BusinessRuleValidationException>()
               .WithMessage(message);
        }
    }
}
