namespace Project.Logging
{
    /// <summary>
    /// Defines the Elasticsearch index configuration for storing general project log entries.
    /// </summary>
    public class BaseLogIndexDefinition : ILogIndexDefinition
    {
        /// <summary>
        /// Gets the name of the Elasticsearch index used for base log entries.
        /// </summary>
        public string IndexName => "project-logs";

        /// <summary>
        /// Returns the mapping definition for the Elasticsearch index, including field types and structure.
        /// </summary>
        /// <returns>An object representing the index mapping configuration.</returns>
        public object GetMapping()
        {
            return new
            {
                mappings = new
                {
                    properties = new
                    {
                        Id = new { type = "keyword" },
                        Timestamp = new { type = "date" },
                        Level = new { type = "keyword" },
                        Message = new { type = "text" },
                        Source = new { type = "keyword" },
                        Metadata = new { type = "object" }
                    }
                }
            };
        }
    }
}
