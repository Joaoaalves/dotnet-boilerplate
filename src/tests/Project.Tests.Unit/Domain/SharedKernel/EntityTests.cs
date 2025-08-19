using System.Reflection;
using Project.Domain.SeedWork;
using Project.Tests.Unit.Domain.Fakes;

namespace Project.Tests.Unit.Domain.SharedKernel
{
    public class EntityTests
    {
        [Fact]
        public void DomainEvents_ShouldBeNull_WhenNoEventsAdded()
        {
            // Arrange
            var entity = new FakeEntity();

            // Act & Assert
            Assert.Null(entity.DomainEvents);
        }

        [Fact]
        public void AddDomainEvent_ShouldInitializeList_IfNull()
        {
            // Arrange
            var entity = new FakeEntity();
            var domainEvent = new FakeDomainEvent();

            // Act
            entity.AddEvent(domainEvent);

            // Assert
            Assert.NotNull(entity.DomainEvents);
            Assert.Single(entity.DomainEvents);
            Assert.Equal(entity.DomainEvents.First(), domainEvent);
        }

        [Fact]
        public void AddDomainEvent_ShouldAddToExistingList()
        {
            // Arrange
            var entity = new FakeEntity();
            var event1 = new FakeDomainEvent();
            var event2 = new FakeDomainEvent();

            entity.AddEvent(event1);

            // Act
            entity.AddEvent(event2);

            // Assert
            Assert.NotNull(entity.DomainEvents);
            Assert.Equal(2, entity.DomainEvents.Count);
            Assert.Equal([event1, event2], entity.DomainEvents);
        }

        [Fact]
        public void ClearDomainEvents_ShouldRemoveAllEvents()
        {
            // Arrange
            var entity = new FakeEntity();
            entity.AddEvent(new FakeDomainEvent());
            entity.AddEvent(new FakeDomainEvent());

            // Act
            entity.ClearDomainEvents();

            // Assert
            Assert.NotNull(entity.DomainEvents);
            Assert.Empty(entity.DomainEvents);
        }

        [Fact]
        public void CheckRule_ShouldNotThrow_WhenRuleIsValid()
        {
            // Arrange
            var entity = new FakeEntity();
            var rule = new FakeValidRule();

            // Act
            var exception = Record.Exception(() => entity.ValidateRule(rule));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void CheckRule_ShouldThrow_WhenRuleIsBroken()
        {
            // Arrange
            var entity = new FakeEntity();
            var rule = new FakeBrokenRule();

            // Assert
            Assert.Throws<BusinessRuleValidationException>(() => entity.ValidateRule(rule));
        }

        [Fact]
        public void DomainEvents_ShouldReturnNull_IfListClearedWithNull()
        {
            // Arrange
            var entity = (FakeEntity)Activator.CreateInstance(typeof(FakeEntity), true)!;

            // Use reflection to set _domainEvents = null explicitly
            var field = typeof(Entity).GetField("_domainEvents", BindingFlags.Instance | BindingFlags.NonPublic);
            field!.SetValue(entity, null);

            // Act
            var result = entity.DomainEvents;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ClearDomainEvents_ShouldHandleNullListGracefully()
        {
            // Arrange
            var entity = (FakeEntity)Activator.CreateInstance(typeof(FakeEntity), true)!;

            // Force _domainEvents to null
            var field = typeof(Entity).GetField("_domainEvents", BindingFlags.Instance | BindingFlags.NonPublic);
            field!.SetValue(entity, null);

            // Act
            var exception = Record.Exception(entity.ClearDomainEvents);

            // Assert
            Assert.Null(exception);
            Assert.Null(entity.DomainEvents);
        }
    }
}