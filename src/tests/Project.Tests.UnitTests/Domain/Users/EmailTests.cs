using System.Reflection;
using FluentAssertions;
using Project.Domain.SeedWork;
using Project.Domain.SharedKernel.Users;
using Project.Domain.SharedKernel.Users.Rules;

namespace Project.Tests.UnitTests.Domain.Users
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
            // Arrange
            var email = new Email(emailStr);

            // Assert
            email.Should().NotBeNull();
            email.Value.Should().Be(emailStr);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalid-email")]
        [InlineData("user@com")]
        public void ShouldNotCreateWithInvalidValue(string emailStr)
        {
            // Arrange
            var message = new EmailMustBeValidRule(emailStr).Message;

            // Act
            Action act = () => new Email(emailStr);

            // Assert
            act.Should()
               .Throw<BusinessRuleValidationException>()
               .WithMessage(message);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenValuesAreEqualIgnoringCase()
        {
            // Arrange
            var email1 = new Email("john@Doe.com");
            var email2 = new Email("jOhn@doe.Com");

            // Act
            var result = email1.Equals(email2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenValuesAreDifferent()
        {
            // Arrange
            var email1 = new Email("john@Doe.com");
            var email2 = new Email("jAne@doe.Com");

            // Act
            var result = email1.Equals(email2);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void Equals_ShouldReturnFalse_WhenComparingWithNull()
        {
            // Arrange
            var email = new Email("john@Doe.com");

            // Act
            var result = email.Equals(null);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void Equals_ShouldReturnFalse_WhenComparingWithDifferentObjectType()
        {
            // Arrange
            var email = new Email("john@Doe.com");
            var other = "john@Doe.com";

            // Act
            var result = email.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ShouldThrow_WhenValueIsNull()
        {
            // Arrange
            var email1 = (Email)Activator.CreateInstance(typeof(Email), true)!;
            var email2 = new Email("any@example.com");

            // Reflection force null Value
            var valueProp = typeof(Email).GetProperty("Value", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            valueProp!.SetValue(email1, null);

            // Act
            Action act = () => email1.Equals(email2);

            // Assert
            act.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void GetHashCode_ShouldBeEqual_ForSameValuesIgnoringCase()
        {
            // Arrange
            var email1 = new Email("john@Doe.com");
            var email2 = new Email("joHn@doe.Com");

            // Act
            var hash1 = email1.GetHashCode();
            var hash2 = email2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnZero_WhenValueIsNull()
        {
            // Arrange
            var email = (Email)Activator.CreateInstance(typeof(Email), true)!;

            // Reflection force Null value
            var valueProp = typeof(Email).GetProperty("Value", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            valueProp!.SetValue(email, null);

            // Act
            var hashCode = email.GetHashCode();

            // Assert
            Assert.Equal(0, hashCode);
        }


        [Fact]
        public void ToString_ShouldReturnEmailValue()
        {
            // Arrange
            var email = new Email("john@Doe.com");

            // Assert
            Assert.Equal(email.ToString(), email.Value);
        }

        [Fact]
        public void ImplicitOperator_ShouldReturnString()
        {
            // Arrange
            Email? email = new("john@doe.com");

            // Act
            string result = email;

            // Assert
            Assert.Equal(result, email.Value);
        }

        [Fact]
        public void ImplicitOperator_ShouldReturnEmptyString_WhenEmailIsNull()
        {
            // Arrange
            Email? email = null;

            // Act
            string result = email!;

            // Assert
            result.Should().BeEmpty();
        }
    }
}
