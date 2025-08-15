namespace Project.Logging.Users
{
    /// <summary>
    /// Defines the Elasticsearch index configuration for logging user-related events.
    /// </summary>
    public class UserLogIndexDefinition : ILogIndexDefinition
    {
        /// <summary>
        /// Gets the name of the Elasticsearch index used for user event logs.
        /// </summary>
        public string IndexName => "user-events";

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
                        EventType = new { type = "keyword" },
                        UserId = new { type = "keyword" },
                        Email = new { type = "keyword" },
                        Metadata = new { type = "object" }
                    }
                }
            };
        }
    }
}
