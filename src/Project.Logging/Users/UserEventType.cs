namespace Project.Logging.Users
{
    /// <summary>
    /// Represents a type of user-related event to be logged.
    /// </summary>
    public sealed class UserEventType(string value)
    {
        /// <summary>
        /// Gets the string value representing the event type.
        /// </summary>
        public string Value { get; private set; } = value;

        /// <summary>
        /// Gets the event type representing user creation.
        /// </summary>
        public static UserEventType UserCreated => new UserEventType("UserCreated");

        /// <summary>
        /// Gets the event type representing user login.
        /// </summary>
        public static UserEventType UserLoggedIn => new UserEventType("UserLoggedIn");

        /// <summary>
        /// Gets the event type representing a password reset.
        /// </summary>
        public static UserEventType UserPasswordReset => new UserEventType("UserPasswordReset");

        /// <summary>
        /// Returns the string representation of the event type.
        /// </summary>
        /// <returns>The string value of the event type.</returns>
        public override string ToString()
        {
            return Value;
        }
    }
}
