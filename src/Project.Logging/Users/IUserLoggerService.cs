using Project.Domain.SharedKernel.Users;

namespace Project.Logging.Users
{
    /// <summary>
    /// Defines a contract for logging user-related events to an external logging system (e.g., Elasticsearch).
    /// </summary>
    public interface IUserLoggerService : ILoggerService<UserLogEntry>
    {
        /// <summary>
        /// Logs an event indicating that a new user has been created.
        /// </summary>
        /// <param name="userId">The unique identifier of the created user.</param>
        /// <param name="email">The email address of the created user.</param>
        /// <returns>A task representing the asynchronous logging operation.</returns>
        Task LogUserCreated(string userId, Email email);

        /// <summary>
        /// Logs an event indicating that a user has successfully logged in.
        /// </summary>
        /// <param name="userId">The unique identifier of the logged-in user.</param>
        /// <param name="email">The email address of the logged-in user.</param>
        /// <returns>A task representing the asynchronous logging operation.</returns>
        Task LogUserLoggedIn(string userId, Email email);

        /// <summary>
        /// Logs an event indicating that a user's password has been reset.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose password was reset.</param>
        /// <param name="email">The email address of the user.</param>
        /// <returns>A task representing the asynchronous logging operation.</returns>
        Task LogUserPasswordReset(string userId, Email email);
    }
}
