using System.Reflection;
using FluentAssertions;
using Project.Domain.SeedWork;
using Project.Domain.SharedKernel.Users;
using Project.Domain.SharedKernel.Users.Rules;

namespace Project.Tests.UnitTests.Domain.UsersTests
{
    public class NameTests
    {
        [Theory]
        [InlineData("John Doe")]
        [InlineData("Kim")]
        [InlineData("Éric")]
        public void ShouldCreate_WithValidName(string nameStr)
        {
            var name = new Name(nameStr);

            name.Should().NotBeNull();
            name.Value.Should().Be(nameStr);
        }

        [Theory]
        [InlineData("J")] // Too short
        [InlineData("Inv@lid")] // Symbols are not allowed
        [InlineData("123")] // Numbers are not allowed
        public void ShouldNotCreate_WithInvalidName(string nameStr)
        {
            var message = new NameMustBeValidRule(nameStr).Message;

            Action act = () => new Name(nameStr);

            act.Should()
                .Throw<BusinessRuleValidationException>()
                .WithMessage(message);
        }


        [Fact]
        public void SetValue_ShouldUpdate_WithValidName()
        {
            // Arrange
            string firstName = "John";
            string updatedName = "Doe";

            // Act
            var name = new Name(firstName);
            name.SetValue(updatedName);

            // Assert
            name.Value.Should().Be(updatedName);
        }


        [Fact]
        public void SetValue_ShouldNotUpdated_WhenNewNameIsInvalid()
        {
            // Arrange
            string validName = "John";
            string invalidName = "123"; // Numbers are not allowed

            // Act
            var message = new NameMustBeValidRule(invalidName).Message;

            var name = new Name(validName);
            Action act = () => name.SetValue(invalidName);


            // Assert
            act.Should().Throw<BusinessRuleValidationException>()
                .WithMessage(message);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenValuesAreEqualIgnoringCase()
        {
            // Arrange
            var name1 = new Name("John Doe");
            var name2 = new Name("john doe");

            // Act
            var result = name1.Equals(name2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenValuesAreDifferent()
        {
            // Arrange
            var name1 = new Name("John Doe");
            var name2 = new Name("Jane Doe");

            // Act
            var result = name1.Equals(name2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenComparingWithNull()
        {
            // Arrange
            var name = new Name("John Doe");

            // Act
            var result = name.Equals(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenComparingWithDifferentObjectType()
        {
            // Arrange
            var name = new Name("John Doe");
            var other = "John Doe"; // string, not a Name

            // Act
            var result = name.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_ShouldBeEqual_ForSameValuesIgnoringCase()
        {
            // Arrange
            var name1 = new Name("John Doe");
            var name2 = new Name("john doe");

            // Act
            var hash1 = name1.GetHashCode();
            var hash2 = name2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnZero_WhenValueIsNull()
        {
            // Arrange
            var name = (Name)Activator.CreateInstance(typeof(Name), true)!;

            // Reflection force Null value
            var valueProp = typeof(Name).GetProperty("Value", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            valueProp!.SetValue(name, null);

            // Act
            var hashCode = name.GetHashCode();

            // Assert
            Assert.Equal(0, hashCode);
        }

        [Fact]
        public void ToString_ShouldReturnNameValue()
        {
            // Arrange
            var name = new Name("John Doe");

            // Assert
            Assert.Equal(name.ToString(), name.Value);
        }
    }
}