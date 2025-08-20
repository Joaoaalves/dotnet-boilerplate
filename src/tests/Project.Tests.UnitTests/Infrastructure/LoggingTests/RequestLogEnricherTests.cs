using Serilog.Core;
using Serilog.Events;
using Project.Application.Configuration;
using Project.Infrastructure.Logging;
using Project.Tests.UnitTests.Application.Fakes;

namespace Project.Tests.UnitTests.Infrastructure.LoggingTests
{
    public class RequestLogEnricherTests
    {
        [Fact]
        public void Enrich_ShouldAddCorrelationId_WhenAvailable()
        {
            var accessor = new FakeExecutionContextAccessor
            {
                IsAvailable = true,
                CorrelationId = Guid.NewGuid()
            };

            var enricher = CreateRequestLogEnricher(accessor);
            var logEvent = CreateLogEvent();

            enricher.Enrich(logEvent, new TestPropertyFactory());

            var property = logEvent.Properties["CorrelationId"] as ScalarValue;
            Assert.NotNull(property);
            Assert.Equal(accessor.CorrelationId, property.Value);
        }

        [Fact]
        public void Enrich_ShouldNotAddCorrelationId_WhenUnavailable()
        {
            var accessor = new FakeExecutionContextAccessor { IsAvailable = false };

            var enricher = CreateRequestLogEnricher(accessor);
            var logEvent = CreateLogEvent();

            enricher.Enrich(logEvent, new TestPropertyFactory());

            Assert.False(logEvent.Properties.ContainsKey("CorrelationId"));
        }

        // Helper to reach the private class
        private static ILogEventEnricher CreateRequestLogEnricher(IExecutionContextAccessor accessor)
        {
            return (ILogEventEnricher)Activator.CreateInstance(
                typeof(LoggingPipelineBehavior<,>)
                    .GetNestedType("RequestLogEnricher", System.Reflection.BindingFlags.NonPublic)!
                    .MakeGenericType(typeof(FakeCommandString), typeof(string)),
                accessor)!;
        }

        private static LogEvent CreateLogEvent() =>
            new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, new MessageTemplate("", []), []);

        private class TestPropertyFactory : ILogEventPropertyFactory
        {
            public LogEventProperty CreateProperty(string name, object? value, bool destructureObjects = false) =>
                new(name, new ScalarValue(value));
        }

    }
}
