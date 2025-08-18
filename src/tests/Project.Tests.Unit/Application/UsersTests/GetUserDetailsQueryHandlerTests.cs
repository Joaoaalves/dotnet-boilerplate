using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Application.Users;
using Project.Application.Users.Queries.GetUserDetails;
using Project.Domain.Users;
using Project.Infrastructure.Domain.Users;
using Project.Tests.Unit.Builders;
using Project.Tests.Unit.Fakes;
using Project.Tests.Unit.Mocks;
using Xunit.Abstractions;

namespace Project.Tests.Unit.Application.UsersTests
{
    public class GetUserDetailsQueryHandlerTests
    {
        private readonly UserManager<User> _userManager;
        private readonly GetUserDetailsQueryHandler _handler;
        private readonly FakeDbContext<User> _dbContext;

        private readonly ITestOutputHelper _output;
        private readonly IUserRepository _userRepo;
        public GetUserDetailsQueryHandlerTests(ITestOutputHelper output)
        {

            _output = output;
            _userManager = UserManagerMock.MockUserManager<User>([]).Object;
            var options = new DbContextOptionsBuilder<FakeDbContext<User>>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new FakeDbContext<User>(options);
            _userRepo = new UserRepository(_userManager, _dbContext);

            _handler = new GetUserDetailsQueryHandler(_userRepo);
        }

        private async Task AddUserToDatabase(User user)
        {
            await _dbContext.AddAsync(user);

            await _dbContext.SaveChangesAsync();
        }


        [Fact]
        public async Task ShouldReturnUserDetails_WhenUserExists()
        {
            // Arrange
            var user = UserBuilder.WithDefaultValues();
            await AddUserToDatabase(user);

            var principal = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new(ClaimTypes.Email, user.Email!)
            ]));

            var query = new GetUserDetailsQuery(principal);
            var res = await _dbContext.Users.ToListAsync();
            _output.WriteLine($"\n\n\n\n\n {res.Count} \n\n\n\n\n");
            // Act
            var dto = await _handler.Handle(query, CancellationToken.None);

            // Assert
            dto.Should().NotBeNull();
            dto!.FirstName.Should().Be(user.FirstName.Value);
            dto.LastName.Should().Be(user.LastName.Value);
            dto.Email.Should().Be(user.Email);
        }

        [Fact]
        public async Task ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Email, "nonexistent@example.com")
            }));

            var query = new GetUserDetailsQuery(principal);

            // Act
            var dto = await _handler.Handle(query, CancellationToken.None);

            // Assert
            dto.Should().BeNull();
        }

        [Fact]
        public async Task ShouldReturnNull_WhenEmailClaimIsMissing()
        {
            // Arrange
            var principal = new ClaimsPrincipal(new ClaimsIdentity());

            var query = new GetUserDetailsQuery(principal);

            // Act
            var dto = await _handler.Handle(query, CancellationToken.None);

            // Assert
            dto.Should().BeNull();
        }
    }
}
