using Nest;
using Project.Domain.SharedKernel.Users;
using Project.Logging;
using Project.Logging.Users;

namespace Project.Infrastructure.Logging.Users
{
    /// <summary>
    /// Elasticsearch-based implementation of <see cref="IUserLoggerService"/> for logging user events.
    /// </summary>
    /// <param name="client">The Elasticsearch client instance.</param>
    /// <param name="indexDefinition">Index definition for user logs.</param>
    public class ElasticUserLoggerService(IElasticClient client, UserLogIndexDefinition indexDefinition) : IUserLoggerService
    {
        private readonly IElasticClient _client = client;
        private readonly ILogIndexDefinition _logIndexDefinition = indexDefinition;

        /// <inheritdoc />
        public async Task LogAsync(UserLogEntry entry)
        {
            var response = await _client.IndexAsync(entry, i => i.Index(_logIndexDefinition.IndexName));

            if (!response.IsValid)
            {
                throw new Exception($"Failed to log user event: {response.OriginalException.Message}");
            }
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
