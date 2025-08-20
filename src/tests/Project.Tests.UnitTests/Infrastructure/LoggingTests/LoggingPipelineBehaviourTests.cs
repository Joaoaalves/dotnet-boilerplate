using Microsoft.Extensions.Logging;
using Project.Application.Configuration;
using Project.Infrastructure.Logging;
using Project.Tests.UnitTests.Application.Fakes;
using Project.Tests.UnitTests.Infrastructure.Fakes;
using Serilog;
using Serilog.Core;
using Serilog.Events;


namespace Project.Tests.UnitTests.Infrastructure.LoggingTests
{
    public class FakeExecutionContextAccessor : IExecutionContextAccessor
    {
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public bool IsAvailable { get; set; } = true;
    }

    public class LoggingPipelineBehaviorTests
    {
        [Fact]
        public async Task Handle_ShouldLogStartAndSuccess_WhenCommandSucceeds()
        {
            var logger = new FakeLogger<LoggingPipelineBehavior<FakeCommandString, string>>();
            var contextAccessor = new FakeExecutionContextAccessor();
            var behavior = new LoggingPipelineBehavior<FakeCommandString, string>(logger, contextAccessor);

            var command = new FakeCommandString();
            Func<Task<string>> next = () => Task.FromResult("ok");

            var result = await behavior.Handle(command, next, CancellationToken.None);

            Assert.Equal("ok", result);

            Assert.Collection(logger.Logs,
                log =>
                {
                    Assert.Equal(LogLevel.Information, log.Level);
                    Assert.Contains("Executing command", log.Message);
                },
                log =>
                {
                    Assert.Equal(LogLevel.Information, log.Level);
                    Assert.Contains("processed successfully", log.Message);
                });
        }

        [Fact]
        public async Task Handle_ShouldLogErrorAndRethrow_WhenCommandThrows()
        {
            var logger = new FakeLogger<LoggingPipelineBehavior<FakeCommandString, string>>();
            var contextAccessor = new FakeExecutionContextAccessor();
            var behavior = new LoggingPipelineBehavior<FakeCommandString, string>(logger, contextAccessor);

            var command = new FakeCommandString();
            Func<Task<string>> next = () => throw new InvalidOperationException("fail");

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                behavior.Handle(command, next, CancellationToken.None));

            Assert.Equal("fail", ex.Message);

            Assert.Collection(logger.Logs,
                log =>
                {
                    Assert.Equal(LogLevel.Information, log.Level);
                    Assert.Contains("Executing command", log.Message);
                },
                log =>
                {
                    Assert.Equal(LogLevel.Error, log.Level);
                    Assert.Contains("processing failed", log.Message);
                    Assert.NotNull(log.Exception);
                });
        }

        [Fact]
        public async Task Handle_ShouldRespectExecutionContext_WhenAvailable()
        {
            var logger = new FakeLogger<LoggingPipelineBehavior<FakeCommandString, string>>();
            var contextAccessor = new FakeExecutionContextAccessor
            {
                IsAvailable = true,
                CorrelationId = Guid.NewGuid()
            };
            var behavior = new LoggingPipelineBehavior<FakeCommandString, string>(logger, contextAccessor);

            var command = new FakeCommandString();
            Func<Task<string>> next = () => Task.FromResult("ok");

            var result = await behavior.Handle(command, next, CancellationToken.None);

            Assert.Equal("ok", result);

            // We can only assert that logs were produced normally.
            Assert.Contains(logger.Logs, l => l.Message.Contains("Executing command"));
        }

        [Fact]
        public async Task Handle_ShouldWork_WhenExecutionContextUnavailable()
        {
            var logger = new FakeLogger<LoggingPipelineBehavior<FakeCommandString, string>>();
            var contextAccessor = new FakeExecutionContextAccessor { IsAvailable = false };
            var behavior = new LoggingPipelineBehavior<FakeCommandString, string>(logger, contextAccessor);

            var command = new FakeCommandString();
            Func<Task<string>> next = () => Task.FromResult("ok");

            var result = await behavior.Handle(command, next, CancellationToken.None);

            Assert.Equal("ok", result);

            Assert.Contains(logger.Logs, l => l.Message.Contains("Executing command"));
        }

        [Fact]
        public async Task Handle_ShouldInvokeCommandLogEnricher()
        {
            // Prepare in-memory sink
            var logEvents = new List<LogEvent>();
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Sink(new DelegatingSink(logEvents.Add))
                .CreateLogger();

            var fakeLogger = new Serilog.Extensions.Logging.SerilogLoggerFactory(logger).CreateLogger<LoggingPipelineBehavior<FakeCommandString, string>>();
            var contextAccessor = new FakeExecutionContextAccessor();
            var behavior = new LoggingPipelineBehavior<FakeCommandString, string>(fakeLogger, contextAccessor);

            var command = new FakeCommandString();
            Func<Task<string>> next = () => Task.FromResult("ok");

            await behavior.Handle(command, next, CancellationToken.None);

            // Check that "Context" property was added
            Assert.Contains(logEvents, e => e.Properties.ContainsKey("Context"));
        }

        // Simple sink delegate to capture events
        private class DelegatingSink : ILogEventSink
        {
            private readonly Action<LogEvent> _write;
            public DelegatingSink(Action<LogEvent> write) => _write = write;
            public void Emit(LogEvent logEvent) => _write(logEvent);
        }
    }
}
