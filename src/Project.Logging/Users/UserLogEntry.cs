using Project.Domain.SharedKernel.Users;

namespace Project.Logging.Users
{
    /// <summary>
    /// Represents a log entry for a user-related event, containing essential user information and event details.
    /// </summary>
    public class UserLogEntry(UserEventType eventType, string userId, Email? email = null) : LogEntry
    {
        /// <summary>
        /// Gets the type of the user event that occurred.
        /// </summary>
        public UserEventType EventType { get; private set; } = eventType;

        /// <summary>
        /// Gets the unique identifier of the user associated with the event.
        /// </summary>
        public string UserId { get; private set; } = userId;

        /// <summary>
        /// Gets the email address of the user associated with the event, if available.
        /// </summary>
        public Email? Email { get; private set; } = email;
    }
}
