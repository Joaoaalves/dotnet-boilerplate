using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using Project.Application.Configuration.Commands;
using Project.Application.Configuration.Commands.Behaviors;
using Project.Application.Users;
using Project.Domain.SeedWork;
using Project.Domain.Users;
using Project.Tests.UnitTests.Builders;

namespace Project.Tests.UnitTests.Application.ConfigurationTests.Commands
{
    public class UserInjectionCommandBehaviorTests
    {
        [Fact]
        public async Task Handle_WhenCommandIsNotUserAware_CallsNextWithoutInjection()
        {
            // Arrange
            var command = new Mock<ICommand<Unit>>().Object;
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var userRepository = new Mock<IUserRepository>();
            var behavior = new UserInjectionCommandBehavior<ICommand<Unit>, Unit>(httpContextAccessor.Object, userRepository.Object);

            var nextCalled = false;
            Task<Unit> Next(ICommand<Unit> cmd)
            {
                nextCalled = true;
                return Task.FromResult(Unit.Value);
            }

            // Act
            var result = await behavior.Handle(command, Next, CancellationToken.None);

            // Assert
            Assert.True(nextCalled);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_WhenUserAware_InsertsUserSuccessfully()
        {
            // Arrange
            var user = UserBuilder.WithDefaultValues();
            var userAwareCommand = new Mock<IUserAware>();
            var command = userAwareCommand.As<ICommand<Unit>>().Object;

            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name, user.Email!)
            }, "mock"));

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(h => h.HttpContext!.User).Returns(claims);

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.GetByEmailAsync(user.Email!)).ReturnsAsync(user);

            var behavior = new UserInjectionCommandBehavior<ICommand<Unit>, Unit>(httpContextAccessor.Object, userRepository.Object);

            var nextCalled = false;
            Task<Unit> Next(ICommand<Unit> cmd)
            {
                nextCalled = true;
                return Task.FromResult(Unit.Value);
            }

            // Act
            var result = await behavior.Handle(command, Next, CancellationToken.None);

            // Assert
            userAwareCommand.Verify(c => c.InjectUser(user), Times.Once);
            Assert.True(nextCalled);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_Throws_WhenHttpContextIsNull()
        {
            // Arrange
            var userAwareCommand = new Mock<IUserAware>();
            var command = userAwareCommand.As<ICommand<Unit>>().Object;

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(h => h.HttpContext).Returns((HttpContext)null!);

            var userRepository = new Mock<IUserRepository>();
            var behavior = new UserInjectionCommandBehavior<ICommand<Unit>, Unit>(httpContextAccessor.Object, userRepository.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                behavior.Handle(command, c => Task.FromResult(Unit.Value), CancellationToken.None)
            );

            Assert.Equal("User not found in context.", ex.Message);
        }

        [Fact]
        public async Task Handle_Throws_WhenIdentityPrincipalIsEmpty()
        {
            // Arrange
            var userAwareCommand = new Mock<IUserAware>();
            var command = userAwareCommand.As<ICommand<Unit>>().Object;

            var claims = new ClaimsPrincipal(); // Empty Principal
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(h => h.HttpContext!.User).Returns(claims);

            var userRepository = new Mock<IUserRepository>();
            var behavior = new UserInjectionCommandBehavior<ICommand<Unit>, Unit>(httpContextAccessor.Object, userRepository.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                behavior.Handle(command, c => Task.FromResult(Unit.Value), CancellationToken.None)
            );

            Assert.Equal("Failed to resolve authenticated user.", ex.Message);
        }

        [Fact]
        public async Task Handle_Throws_WhenUserRepositoryReturnsNull()
        {
            // Arrange
            var userAwareCommand = new Mock<IUserAware>();
            var command = userAwareCommand.As<ICommand<Unit>>().Object;

            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name, "test@example.com")
            }, "mock"));

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(h => h.HttpContext!.User).Returns(claims);

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync((User)null!);

            var behavior = new UserInjectionCommandBehavior<ICommand<Unit>, Unit>(httpContextAccessor.Object, userRepository.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                behavior.Handle(command, c => Task.FromResult(Unit.Value), CancellationToken.None)
            );

            Assert.Equal("Failed to resolve authenticated user from repository.", ex.Message);
        }
    }

}
