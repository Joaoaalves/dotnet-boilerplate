using Microsoft.AspNetCore.Http;
using Project.Application.Configuration.Commands;
using Project.Application.Configuration.Queries.Behaviors;
using Project.Tests.UnitTests.Application.Dummies;
using Project.Tests.UnitTests.Application.Fakes;
using Project.Tests.UnitTests.Builders;
using Project.Tests.UnitTests.Infrastructure.Fakes;

namespace Project.Tests.UnitTests.Application.ConfigurationTests.Queries
{
    public class UserInjectionQueryBehaviorTests
    {
        [Fact]
        public async Task Handle_WhenQueryIsNotUserAware_CallsNextWithoutInjection()
        {
            var query = new DummyQuery();
            var httpContextAccessor = FakeHttpContextAccessor.WithEmail("test@example.com");
            var userRepository = new FakeUserRepository();
            var behavior = new UserInjectionQueryBehavior<DummyQuery, Unit>(
                httpContextAccessor,
                userRepository
            );

            var nextCalled = false;
            Task<Unit> Next()
            {
                nextCalled = true;
                return Task.FromResult(Unit.Value);
            }

            var result = await behavior.Handle(query, Next, CancellationToken.None);

            Assert.True(nextCalled);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_WhenUserAware_InsertsUserSuccessfully()
        {
            var user = UserBuilder.WithDefaultValues();
            var query = new FakeUserAwareQuery<Unit>();
            var httpContextAccessor = FakeHttpContextAccessor.WithEmail(user.Email!);
            var userRepository = new FakeUserRepository(user);
            var behavior = new UserInjectionQueryBehavior<FakeUserAwareQuery<Unit>, Unit>(
                httpContextAccessor,
                userRepository
            );

            var result = await behavior.Handle(query, () => Task.FromResult(Unit.Value), CancellationToken.None);

            Assert.Equal(user, query.InjectedUser);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_Throws_WhenHttpContextIsNull()
        {
            var query = new FakeUserAwareQuery<Unit>();
            var httpContextAccessor = FakeHttpContextAccessor.Empty();
            var userRepository = new FakeUserRepository();
            var behavior = new UserInjectionQueryBehavior<FakeUserAwareQuery<Unit>, Unit>(
                httpContextAccessor,
                userRepository
            );

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                behavior.Handle(query, () => Task.FromResult(Unit.Value), CancellationToken.None)
            );

            Assert.Equal("User not found in context.", ex.Message);
        }

        [Fact]
        public async Task Handle_Throws_WhenEmailClaimIsMissing()
        {
            var query = new FakeUserAwareQuery<Unit>();
            var httpContextAccessor = new FakeHttpContextAccessor
            {
                HttpContext = new DefaultHttpContext() // sem claim
            };
            var userRepository = new FakeUserRepository();
            var behavior = new UserInjectionQueryBehavior<FakeUserAwareQuery<Unit>, Unit>(
                httpContextAccessor,
                userRepository
            );

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                behavior.Handle(query, () => Task.FromResult(Unit.Value), CancellationToken.None)
            );

            Assert.Equal("Email not found on Claims", ex.Message);
        }

        [Fact]
        public async Task Handle_Throws_WhenUserRepositoryReturnsNull()
        {
            var query = new FakeUserAwareQuery<Unit>();
            var httpContextAccessor = FakeHttpContextAccessor.WithEmail("test@example.com");
            var userRepository = new FakeUserRepository(null);
            var behavior = new UserInjectionQueryBehavior<FakeUserAwareQuery<Unit>, Unit>(
                httpContextAccessor,
                userRepository
            );

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                behavior.Handle(query, () => Task.FromResult(Unit.Value), CancellationToken.None)
            );

            Assert.Equal("Failed to resolve authenticated user.", ex.Message);
        }
    }
}