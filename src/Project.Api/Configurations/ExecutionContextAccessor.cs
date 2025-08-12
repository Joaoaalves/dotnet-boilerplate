using Project.Application.Configuration;

namespace Project.API.Configurations
{
    /// <summary>
    /// Provides access to the current execution context, specifically the correlation ID from the HTTP request.
    /// </summary>
    public class ExecutionContextAccessor : IExecutionContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionContextAccessor"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">An accessor to retrieve the current HTTP context.</param>
        public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the correlation ID from the incoming HTTP request headers.
        /// </summary>
        /// <exception cref="ApplicationException">
        /// Thrown if the HTTP context or correlation ID is not available in the request.
        /// </exception>
        public Guid CorrelationId
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext == null)
                {
                    throw new ApplicationException("HTTP context is not available.");
                }

                if (!httpContext.Request.Headers.TryGetValue(CorrelationMiddleware.CorrelationHeaderKey, out var correlationIdHeader))
                {
                    throw new ApplicationException("Correlation ID is not available in the request headers.");
                }

                if (!Guid.TryParse(correlationIdHeader, out var correlationId))
                {
                    throw new ApplicationException("Invalid Correlation ID format.");
                }

                return correlationId;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the HTTP context is currently available.
        /// </summary>
        public bool IsAvailable => _httpContextAccessor.HttpContext != null;
    }
}
