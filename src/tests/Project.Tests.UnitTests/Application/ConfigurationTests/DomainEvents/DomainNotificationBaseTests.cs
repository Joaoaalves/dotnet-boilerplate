using Project.Application.Configuration.DomainEvents;
using Project.Tests.UnitTests.Domain.Fakes;

namespace Project.Tests.UnitTests.Application.ConfigurationTests.DomainEvents
{
    public class DomainNotificationBaseTests
    {
        [Fact]
        public void Constructor_ShouldSetDomainEvent()
        {
            // Arrange
            var domainEvent = new FakeDomainEvent();

            // Act
            var notification = new DomainNotificationBase<FakeDomainEvent>(domainEvent);

            // Assert
            Assert.Equal(domainEvent, notification.DomainEvent);
        }

        [Fact]
        public void Constructor_ShouldGenerateId()
        {
            // Arrange
            var domainEvent = new FakeDomainEvent();

            // Act
            var notification = new DomainNotificationBase<FakeDomainEvent>(domainEvent);

            // Assert
            Assert.NotEqual(Guid.Empty, notification.Id);
        }
    }
}