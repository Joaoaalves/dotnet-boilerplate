using Project.Application.Users.Mappers;
using Project.Tests.UnitTests.Builders;

namespace Project.Tests.UnitTests.Application.UsersTests
{
    public class UserMapperTests
    {
        [Fact]
        public void ToUserDetailsDTO_ShouldReturnCorrectValues_WhenAllDataIsCorrect()
        {
            // Arrange
            var user = UserBuilder.WithDefaultValues();

            // Act
            var dto = user.ToUserDetailsDTO();

            // Assert
            Assert.Equal(user.Email, dto.Email);
            Assert.Equal(user.FirstName.Value, dto.FirstName);
            Assert.Equal(user.LastName.Value, dto.LastName);
        }

        [Fact]
        public void ToUserDetailsDTO_ShouldReturnEmptyEmail_WhenNoEmailWasProvided()
        {
            // Arrange
            var user = UserBuilder.WithDefaultValues();
            user.Email = null;

            // Act
            var dto = user.ToUserDetailsDTO();

            // Assert
            Assert.NotNull(dto.Email);
            Assert.Empty(dto.Email);
        }
    }
}