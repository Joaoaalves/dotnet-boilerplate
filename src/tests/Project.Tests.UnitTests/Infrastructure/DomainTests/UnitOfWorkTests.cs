
using Microsoft.EntityFrameworkCore;
using Moq;
using Project.Domain.Users;
using Project.Infrastructure.Domain;
using Project.Infrastructure.Processing;
using Project.Tests.UnitTests.Builders;
using Project.Tests.UnitTests.Domain.Fakes;
using Project.Tests.UnitTests.Infrastructure.Fakes;

namespace Project.Tests.UnitTests.Infrastructure.DomainTests
{
    public class UnitOfWorkTests
    {
        private readonly FakeDbContext<User> _context;
        private readonly Mock<IDomainEventsDispatcher> _dispatcherMock;

        public UnitOfWorkTests()
        {
            _context = DatabaseBuilder.InMemoryDatabase();
            _dispatcherMock = new Mock<IDomainEventsDispatcher>();
        }

        [Fact]
        public async Task CommitAsync_ShouldDispatchEventsAndSaveChanges()
        {
            // Arrange
            _dispatcherMock.Setup(d => d.DispatchEventsAsync()).Returns(Task.CompletedTask);
            var uow = new UnitOfWork(_context, _dispatcherMock.Object);
            _context.FakeEntities.Add(new FakeEntity());

            // Act
            var result = await uow.CommitAsync();

            // Assert
            Assert.Equal(1, result);
            _dispatcherMock.Verify(d => d.DispatchEventsAsync(), Times.Once);
            Assert.Equal(0, await _context.SaveChangesAsync());
        }

        [Fact]
        public async Task RevertAsync_ShouldDetachAddedEntities()
        {
            // Arrange
            var uow = new UnitOfWork(_context, _dispatcherMock.Object);
            var addedEntity = new FakeEntity("Added");
            _context.FakeEntities.Add(addedEntity);

            // Act
            await uow.RevertAsync();

            // Assert
            var addedEntry = _context.Entry(addedEntity);
            Assert.Equal(EntityState.Detached, addedEntry.State);
            Assert.DoesNotContain(_context.ChangeTracker.Entries(), e => e.Entity == addedEntity);
        }

        [Fact]
        public async Task RevertAsync_ShouldResetModifiedEntitiesToUnchanged()
        {
            // Arrange
            var uow = new UnitOfWork(_context, _dispatcherMock.Object);
            var modifiedEntity = new FakeEntity("Modified");
            _context.FakeEntities.Add(modifiedEntity);
            await _context.SaveChangesAsync();
            modifiedEntity.Name = "Changed";

            // Act
            await uow.RevertAsync();

            // Assert
            var modifiedEntry = _context.Entry(modifiedEntity);
            Assert.Equal(EntityState.Unchanged, modifiedEntry.State);
            Assert.Equal("Modified", modifiedEntity.Name);
        }

        [Fact]
        public async Task RevertAsync_ShouldResetDeletedEntitiesToUnchanged()
        {
            // Arrange
            var uow = new UnitOfWork(_context, _dispatcherMock.Object);
            var deletedEntity = new FakeEntity("Deleted");
            _context.FakeEntities.Add(deletedEntity);
            await _context.SaveChangesAsync();
            _context.FakeEntities.Remove(deletedEntity);

            // Act
            await uow.RevertAsync();

            // Assert
            var deletedEntry = _context.Entry(deletedEntity);
            Assert.Equal(EntityState.Unchanged, deletedEntry.State);
        }

        [Fact]
        public async Task RevertAsync_ShouldLeaveUnchangedEntitiesUnchanged()
        {
            // Arrange
            var uow = new UnitOfWork(_context, _dispatcherMock.Object);
            var unchangedEntity = new FakeEntity("Unchanged");
            _context.FakeEntities.Add(unchangedEntity);
            await _context.SaveChangesAsync();

            // Act
            await uow.RevertAsync();

            // Assert
            var unchangedEntry = _context.Entry(unchangedEntity);
            Assert.Equal(EntityState.Unchanged, unchangedEntry.State);
        }

        [Fact]
        public async Task RevertAsync_ShouldClearAllPendingChanges()
        {
            // Arrange
            var uow = new UnitOfWork(_context, _dispatcherMock.Object);
            _context.FakeEntities.Add(new FakeEntity("A"));
            var entity = new FakeEntity("B");
            _context.FakeEntities.Add(entity);
            await _context.SaveChangesAsync();
            entity.Name = "Changed";

            _context.FakeEntities.Remove(new FakeEntity("ToDelete"));

            // Act
            await uow.RevertAsync();
            var changes = await _context.SaveChangesAsync();

            // Assert
            Assert.Equal(0, changes);
        }
    }

}