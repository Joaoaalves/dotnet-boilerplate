using Project.Domain.SharedKernel.Users;
using Project.Infrastructure.Logging.Users;

namespace Project.Tests.UnitTests.Infrastructure.LoggingTests.Users
{
    public class DefaultUserLoggerServiceTests
    {
        [Fact]
        public async Task LogUserCreated_ShouldCallLogAsync()
        {
            var service = new DefaultUserLoggerService();
            var email = new Email("test@example.com");

            await service.LogUserCreated("user-1", email);
            await service.LogUserLoggedIn("user-1", email);
            await service.LogUserPasswordReset("user-1", email);
        }
    }
}
