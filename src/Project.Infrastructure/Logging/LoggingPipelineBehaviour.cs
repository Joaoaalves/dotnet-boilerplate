using Microsoft.Extensions.Logging;
using Project.Application.Configuration;
using Project.Application.Configuration.Commands;
using Project.Domain.SeedWork;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Project.Infrastructure.Logging
{
    /// <summary>
    /// Pipeline behavior for logging command execution within the application.
    /// Logs execution start, success, and failure events using Serilog.
    /// </summary>
    /// <typeparam name="TRequest">The type of the command.</typeparam>
    /// <typeparam name="TResponse">The type of the command response.</typeparam>
    public class LoggingPipelineBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;
        private readonly IExecutionContextAccessor _contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingPipelineBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger, IExecutionContextAccessor contextAccessor)
        {
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken)
        {
            using (LogContext.Push(
                new RequestLogEnricher(_contextAccessor),
                new CommandLogEnricher(request)))
            {
                var commandName = request.GetType().Name;

                try
                {
                    _logger.LogInformation("Executing command {Command}.", commandName);
                    var result = await next();
                    _logger.LogInformation("Command {Command} processed successfully", commandName);
                    return result;
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc, "Command {Command} processing failed", commandName);
                    throw;
                }
            }
        }

        private class CommandLogEnricher(ICommand<TResponse> command) : ILogEventEnricher
        {
            private readonly ICommand<TResponse> _command = command;

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Command:{_command.Id.ToString()}")));
            }
        }

        private class RequestLogEnricher(IExecutionContextAccessor contextAccessor) : ILogEventEnricher
        {
            private readonly IExecutionContextAccessor _contextAccessor = contextAccessor;

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                if (_contextAccessor.IsAvailable)
                {
                    logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(_contextAccessor.CorrelationId)));
                }
            }
        }
    }
}
