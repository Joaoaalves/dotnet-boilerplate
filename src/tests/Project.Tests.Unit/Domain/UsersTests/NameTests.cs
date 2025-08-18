
using FluentAssertions;
using Project.Domain.SeedWork;
using Project.Domain.SharedKernel.Users;
using Project.Domain.SharedKernel.Users.Rules;

namespace Project.Tests.Unit.Domain.UsersTests
{
    public class NameTests
    {
        [Theory]
        [InlineData("John Doe")]
        [InlineData("Kim")]
        [InlineData("Éric")]
        public void ShouldCreateWithValidValue(string nameStr)
        {
            var name = new Name(nameStr);

            name.Should().NotBeNull();
            name.Value.Should().Be(nameStr);
        }

        [Theory]
        [InlineData("J")] // Too short
        [InlineData("Inv@lid")] // Symbols are not allowed
        [InlineData("123")] // Numbers are not allowed
        public void ShouldNotCreateWithInvalidValue(string nameStr)
        {
            var message = new NameMustBeValidRule(nameStr).Message;

            Action act = () => new Name(nameStr);

            act.Should()
                .Throw<BusinessRuleValidationException>()
                .WithMessage(message);
        }


        [Fact]
        public void ShouldUpdateNameWithValidValue()
        {
            string firstName = "John";
            string updatedName = "Doe";

            var name = new Name(firstName);
            name.SetValue(updatedName);

            name.Value.Should().Be(updatedName);
        }


        [Fact]
        public void ShouldNotUpdateNameWithInvalidValue()
        {
            string validName = "John";
            string invalidName = "123"; // Numbers are not allowed

            var message = new NameMustBeValidRule(invalidName).Message;

            var name = new Name(validName);
            Action act = () => name.SetValue(invalidName);

            act.Should().Throw<BusinessRuleValidationException>()
                .WithMessage(message);
        }
    }
}