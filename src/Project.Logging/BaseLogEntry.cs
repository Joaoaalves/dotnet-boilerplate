namespace Project.Logging
{
    /// <summary>
    /// Represents a base log entry with common log information such as level, message, and source.
    /// Inherits from <see cref="LogEntry"/>.
    /// </summary>
    /// <param name="level">The severity level of the log (e.g., Info, Warning, Error).</param>
    /// <param name="message">The message content of the log entry.</param>
    /// <param name="source">The source or component where the log was generated.</param>
    public class BaseLogEntry(string level, string message, string source) : LogEntry
    {
        /// <summary>
        /// Gets the severity level of the log entry.
        /// </summary>
        public string Level { get; private set; } = level;

        /// <summary>
        /// Gets the message content of the log entry.
        /// </summary>
        public string Message { get; private set; } = message;

        /// <summary>
        /// Gets the source or component that generated the log entry.
        /// </summary>
        public string Source { get; private set; } = source;
    }
}
