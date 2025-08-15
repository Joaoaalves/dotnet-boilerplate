namespace Project.Logging
{
    /// <summary>
    /// Defines the structure of an Elasticsearch index for storing logs.
    /// </summary>
    public interface ILogIndexDefinition
    {
        /// <summary>
        /// Gets the name of the Elasticsearch index.
        /// </summary>
        string IndexName { get; }

        /// <summary>
        /// Returns the mapping configuration for the Elasticsearch index.
        /// </summary>
        /// <returns>An object representing the index mapping.</returns>
        object GetMapping();
    }
}
