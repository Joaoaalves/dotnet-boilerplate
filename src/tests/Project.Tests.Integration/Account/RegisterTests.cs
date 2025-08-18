using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Project.Application.Users.Commands.RegisterUser;

namespace Project.Tests.Integration.Account
{
    public class RegisterTests(SharedDatabaseFixture fixture) : IntegrationTestBase(fixture)
    {
        [Theory]
        [InlineData("John", "Doe", "john@example.com", "Str0ngP@ss")]
        public async Task ShouldCreateUserWithValidName(string firstName, string lastName, string email, string password)
        {
            var command = new RegisterUserCommand(
                firstName: firstName,
                lastName: lastName,
                email: email,
                password: password
            );

            var response = await Client.PostAsJsonAsync("/api/register", command);
            response.StatusCode.Should().Be(HttpStatusCode.Created);


            await ValidateUser(firstName, lastName, email, password);
        }

        [Theory]
        [InlineData("Jo", "Doe", "jo@example.com", "Str0ngP@ss")] // first name < 3 chars
        [InlineData("John", "D@", "john@example.com", "Str0ngP@ss")] // invalid char in last name
        [InlineData("Jane", "123", "jane@example.com", "Str0ngP@ss")] // numeric last name
        public async Task ShouldNotCreateUserWithInvalidName(string firstName, string lastName, string email, string password)
        {
            var command = new RegisterUserCommand(
                firstName: firstName,
                lastName: lastName,
                email: email,
                password: password
            );

            var response = await Client.PostAsJsonAsync("/api/register", command);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("John", "Doe", "john1@example.com", "123")] // too short
        [InlineData("Jane", "Doe", "jane1@example.com", "password")] // common word
        public async Task ShouldNotCreateUserWithWeakPassword(string firstName, string lastName, string email, string password)
        {
            var command = new RegisterUserCommand(
                firstName: firstName,
                lastName: lastName,
                email: email,
                password: password
            );

            var response = await Client.PostAsJsonAsync("/api/register", command);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ShouldNotCreateUserWithDuplicateEmail()
        {
            var command = new RegisterUserCommand(
                firstName: "John",
                lastName: "Doe",
                email: "duplicate@example.com",
                password: "Str0ngP@ss"
            );

            var response1 = await Client.PostAsJsonAsync("/api/register", command);
            response1.StatusCode.Should().Be(HttpStatusCode.Created);

            await ValidateUser(command.FirstName, command.LastName, command.Email, command.Password);

            var response2 = await Client.PostAsJsonAsync("/api/register", command);
            response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("Ana Maria", "Silva", "ana@example.com", "Str0ngP@ss")] // spaces in first name
        [InlineData("Éric", "Dupont", "eric@example.com", "Str0ngP@ss")] // accented chars
        public async Task ShouldAllowValidEdgeCaseNames(string firstName, string lastName, string email, string password)
        {
            var command = new RegisterUserCommand(
                firstName: firstName,
                lastName: lastName,
                email: email,
                password: password
            );

            var response = await Client.PostAsJsonAsync("/api/register", command);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            await ValidateUser(firstName, lastName, email, password);
        }

        private async Task ValidateUser(string firstName, string lastName, string email, string password)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            user.Should().NotBeNull();
            user.FirstName.ToString().Should().Be(firstName);
            user.LastName.ToString().Should().Be(lastName);
            user.Email.Should().Be(email);
            user.UserName.Should().Be(email);
            user.PasswordHash.Should().NotBeNullOrEmpty();

            var response = await Client.PostAsJsonAsync("/login", new
            {
                Email = email,
                Password = password
            });

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("accessToken");
        }
    }
}
