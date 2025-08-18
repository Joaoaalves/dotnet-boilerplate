using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Project.Application.Users.Commands.RegisterUser;
using Project.Application.Users.Queries.GetUserDetails;

namespace Project.Tests.Integration.Account
{
    /// <summary>
    /// Integration tests for the /api/info endpoint.
    /// These tests ensure authentication and user retrieval logic work correctly.
    /// </summary>
    public class InfoTests(SharedDatabaseFixture fixture) : IntegrationTestBase(fixture)
    {
        /// <summary>
        /// Helper method that registers a user and simulates authentication.
        /// Instead of performing a real login, we rely on the test pipeline
        /// and <see cref="IntegrationTestBase.AuthenticateClient"/> to attach
        /// a fake authentication header.
        /// </summary>
        private async Task<string> RegisterAndAuthenticate(
            string firstName = "Ana",
            string lastName = "Silva",
            string email = "ana.silva@example.com",
            string password = "Str0ngP@ss")
        {
            var command = new RegisterUserCommand(firstName, lastName, email, password);
            var response = await Client.PostAsJsonAsync("/api/register", command);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            return email; // we will later authenticate using this email
        }

        [Fact]
        public async Task ShouldReturnUserInfo_WhenAuthenticated()
        {
            var email = await RegisterAndAuthenticate();
            AuthenticateClient(email);

            var response = await Client.GetAsync("/api/info");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var dto = await response.Content.ReadFromJsonAsync<UserDetailsDTO>();
            dto.Should().NotBeNull();
            dto!.Email.Should().Be(email);
            dto.FirstName.Should().Be("Ana");
            dto.LastName.Should().Be("Silva");
        }

        [Fact]
        public async Task ShouldReturnUnauthorized_WhenNotAuthenticated()
        {
            var response = await Client.GetAsync("/api/info");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnUnauthorized_WhenTokenIsInvalid()
        {
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "fake.invalid.token");

            var response = await Client.GetAsync("/api/info");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnUnauthorized_WhenUserNoLongerExists()
        {
            var email = await RegisterAndAuthenticate();
            AuthenticateClient(email);

            // Remove the user directly from the database
            var user = await DbContext.Users.FirstAsync(u => u.Email == email);
            DbContext.Users.Remove(user);
            await DbContext.SaveChangesAsync();

            var response = await Client.GetAsync("/api/info");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ShouldRespectAuthorizationScheme()
        {
            var email = await RegisterAndAuthenticate();
            AuthenticateClient(email);

            // Replace the Bearer scheme with an invalid scheme
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", "abc123");

            var response = await Client.GetAsync("/api/info");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnCorrectClaimsData()
        {
            var email = await RegisterAndAuthenticate(
                firstName: "Éric",
                lastName: "Dupont",
                email: "eric@example.com");

            AuthenticateClient(email);

            var response = await Client.GetAsync("/api/info");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var dto = await response.Content.ReadFromJsonAsync<UserDetailsDTO>();
            dto.Should().NotBeNull();
            dto!.Email.Should().Be(email);
            dto.FirstName.Should().Be("Éric");
            dto.LastName.Should().Be("Dupont");
        }
    }
}
