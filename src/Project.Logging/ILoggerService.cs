namespace Project.Logging
{
    /// <summary>
    /// Defines a contract for logging entries of type <typeparamref name="T"/> to an external system.
    /// </summary>
    /// <typeparam name="T">The type of log entry being logged, which must implement <see cref="ILogEntry"/>.</typeparam>
    public interface ILoggerService<T> where T : ILogEntry
    {
        /// <summary>
        /// Logs the specified entry asynchronously.
        /// </summary>
        /// <param name="entry">The log entry to be recorded.</param>
        /// <returns>A task representing the asynchronous logging operation.</returns>
        Task LogAsync(T entry);
    }
}
