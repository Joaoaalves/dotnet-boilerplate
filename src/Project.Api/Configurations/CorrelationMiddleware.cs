namespace Project.API.Configurations
{
    /// <summary>
    /// Middleware that adds a unique correlation ID to each HTTP request.
    /// </summary>
    internal class CorrelationMiddleware
    {
        /// <summary>
        /// The name of the HTTP header used to store the correlation ID.
        /// </summary>
        internal const string CorrelationHeaderKey = "CorrelationId";

        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware component in the request pipeline.</param>
        public CorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Adds a correlation ID to the request headers and invokes the next middleware.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task that represents the completion of request processing.</returns>
        public async Task Invoke(HttpContext context)
        {
            var correlationId = Guid.NewGuid();

            context.Request?.Headers.Append(CorrelationHeaderKey, correlationId.ToString());

            await _next.Invoke(context);
        }
    }
}
