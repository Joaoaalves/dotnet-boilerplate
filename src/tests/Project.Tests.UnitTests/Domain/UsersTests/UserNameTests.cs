using System.Reflection;
using FluentAssertions;
using Project.Domain.SeedWork;
using Project.Domain.SharedKernel.Users;
using Project.Domain.SharedKernel.Users.Rules;

namespace Project.Tests.UnitTests.Domain.UsersTests
{
    public class UserNameTests
    {
        [Theory]
        [InlineData("john_doe@username.com")]
        [InlineData("user123@username.com")]
        [InlineData("ÉricDupont@username.com")]
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

        [Fact]
        public void Equals_ShouldReturnTrue_WhenValuesAreEqualIgnoringCase()
        {
            // Arrange
            var email1 = new UserName("john@Doe.com");
            var email2 = new UserName("jOhn@doe.Com");

            // Act
            var result = email1.Equals(email2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenValuesAreDifferent()
        {
            // Arrange
            var email1 = new UserName("john@Doe.com");
            var email2 = new UserName("jAne@doe.Com");

            // Act
            var result = email1.Equals(email2);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void Equals_ShouldReturnFalse_WhenComparingWithNull()
        {
            // Arrange
            var username = new UserName("john@Doe.com");

            // Act
            var result = username.Equals(null);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void Equals_ShouldReturnFalse_WhenComparingWithDifferentObjectType()
        {
            // Arrange
            var username = new UserName("john@Doe.com");
            var other = "john@Doe.com";

            // Act
            var result = username.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ShouldThrow_WhenValueIsNull()
        {
            // Arrange
            var email1 = (UserName)Activator.CreateInstance(typeof(UserName), true)!;
            var email2 = new UserName("any@example.com");

            // Reflection force null Value
            var valueProp = typeof(UserName).GetProperty("Value", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
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
            var email1 = new UserName("john@Doe.com");
            var email2 = new UserName("joHn@doe.Com");

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
            var username = (UserName)Activator.CreateInstance(typeof(UserName), true)!;

            // Reflection force Null value
            var valueProp = typeof(UserName).GetProperty("Value", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            valueProp!.SetValue(username, null);

            // Act
            var hashCode = username.GetHashCode();

            // Assert
            Assert.Equal(0, hashCode);
        }


        [Fact]
        public void ToString_ShouldReturnUserNameValue()
        {
            // Arrange
            var username = new UserName("john@Doe.com");

            // Assert
            Assert.Equal(username.ToString(), username.Value);
        }

        [Fact]
        public void ImplicitOperator_ShouldReturnEmptyString_WhenUserNameIsNull()
        {
            // Arrange
            UserName? username = null;

            // Act
            string result = username!;

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ImplicitOperator_ShouldReturnString()
        {
            // Arrange
            UserName? username = new("john@doe.com");

            // Act
            string result = username;

            // Assert
            Assert.Equal(result, username.Value);
        }
    }
}