namespace Project.Logging
{
    /// <summary>
    /// Represents a base interface for a log entry.
    /// </summary>
    public interface ILogEntry
    {
        /// <summary>
        /// Adds a dictionary of metadata to the log entry.
        /// </summary>
        /// <param name="metadata">A dictionary containing metadata key-value pairs.</param>
        void AddMetadata(Dictionary<string, object> metadata);
    }
}