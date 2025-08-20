using FluentAssertions;
using Project.Application.Users.Queries.GetUserDetails;

namespace Project.Tests.UnitTests.Application.UsersTests.GetUserDetailsQueryTests
{
    public class GetUserDetailsQueryTests
    {
        [Theory]
        [InlineData("email@test.com")]
        [InlineData("q@q.c")]
        [InlineData("email@with.subdomain.com")]
        public void ShouldReturnEmail_WhenEmailIsProvided(string email)
        {
            // Arrange
            var command = new GetUserDetailsQuery(email);

            // Assert
            command.Email.Should().NotBeNullOrEmpty();
            command.Email.Should().Be(email);
        }

        [Fact]
        public void ShouldReturnNull_WhenNoEmailIsProvided()
        {
            // Arrange
            var command = new GetUserDetailsQuery();

            //Assert
            command.Email.Should().BeNullOrEmpty();
        }
    }
}