using Project.Domain.SharedKernel.Users;
using Project.Logging.Users;

namespace Project.Infrastructure.Logging.Users
{
    /// <summary>
    /// Default implementation of <see cref="IUserLoggerService"/> that logs user events to the console.
    /// Used when logging is disabled or Elasticsearch is not configured.
    /// </summary>
    public class DefaultUserLoggerService : IUserLoggerService
    {
        /// <inheritdoc />
        public Task LogAsync(UserLogEntry entry)
        {
            Console.WriteLine($"[USER_LOG] - EventType: {entry.EventType.Value} | UserId: {entry.UserId} | Email: {entry.Email}");
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task LogUserCreated(string userId, Email email)
        {
            var logEntry = new UserLogEntry(
                UserEventType.UserCreated,
                userId,
                email
            );

            return LogAsync(logEntry);
        }

        /// <inheritdoc />
        public Task LogUserLoggedIn(string userId, Email email)
        {
            var logEntry = new UserLogEntry(
                UserEventType.UserLoggedIn,
                userId,
                email
            );

            return LogAsync(logEntry);
        }

        /// <inheritdoc />
        public Task LogUserPasswordReset(string userId, Email email)
        {
            var logEntry = new UserLogEntry(
                UserEventType.UserPasswordReset,
                userId,
                email
            );

            return LogAsync(logEntry);
        }
    }
}
