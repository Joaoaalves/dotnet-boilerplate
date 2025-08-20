using Project.Domain.Users;
using Project.Infrastructure.Processing;
using Project.Tests.UnitTests.Builders;
using Project.Tests.UnitTests.Domain.Fakes;
using Project.Tests.UnitTests.Infrastructure.Fakes;
using Project.Tests.UnitTests.Infrastructure.Fakes.Project.Tests.UnitTests.Infrastructure.ProcessingTests;

namespace Project.Tests.UnitTests.Infrastructure.ProcessingTests
{

    public class DomainEventsDispatcherTests
    {
        private readonly FakeDbContext<User> _context;

        public DomainEventsDispatcherTests()
        {
            _context = DatabaseBuilder.InMemoryDatabase();
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenMediatorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new DomainEventsDispatcher(null!, _context));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenContextIsNull()
        {
            var mediator = new FakeMediator();

            Assert.Throws<ArgumentNullException>(() => new DomainEventsDispatcher(mediator, null!));
        }

        [Fact]
        public async Task DispatchEventsAsync_ShouldDoNothing_WhenNoEntities()
        {
            var mediator = new FakeMediator();
            var dispatcher = new DomainEventsDispatcher(mediator, _context);

            await dispatcher.DispatchEventsAsync();

            Assert.Empty(mediator.PublishedEvents);
        }

        [Fact]
        public async Task DispatchEventsAsync_ShouldIgnoreEntitiesWithoutEvents()
        {
            var mediator = new FakeMediator();
            var dispatcher = new DomainEventsDispatcher(mediator, _context);

            _context.FakeEntities.Add(new FakeEntity());
            await _context.SaveChangesAsync();

            await dispatcher.DispatchEventsAsync();

            Assert.Empty(mediator.PublishedEvents);
        }

        [Fact]
        public async Task DispatchEventsAsync_ShouldPublishDomainEvents_AndClearThem()
        {
            var mediator = new FakeMediator();
            var dispatcher = new DomainEventsDispatcher(mediator, _context);

            var entity = new FakeEntity();
            entity.AddEvent(new FakeDomainEvent());
            entity.AddEvent(new FakeDomainEvent());

            _context.FakeEntities.Add(entity);
            await _context.SaveChangesAsync();

            await dispatcher.DispatchEventsAsync();

            Assert.Equal(2, mediator.PublishedEvents.Count);
            Assert.Empty(entity.DomainEvents ?? []);
        }
    }
}