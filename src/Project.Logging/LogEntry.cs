namespace Project.Logging
{
    /// <summary>
    /// Represents a generic log entry with an ID, timestamp, and optional metadata.
    /// Implements <see cref="ILogEntry"/>.
    /// </summary>
    public class LogEntry : ILogEntry
    {
        /// <summary>
        /// Gets the unique identifier for the log entry.
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the timestamp of when the log entry was created (UTC).
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets the metadata associated with the log entry.
        /// </summary>
        public Dictionary<string, object>? Metadata { get; private set; }

        /// <summary>
        /// Adds a dictionary of metadata to the log entry.
        /// </summary>
        /// <param name="metadata">A dictionary containing metadata key-value pairs.</param>
        public void AddMetadata(Dictionary<string, object> metadata)
        {
            Metadata = metadata;
        }
    }
}