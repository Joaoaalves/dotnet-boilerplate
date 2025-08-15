using Project.Domain.SeedWork;
using Project.Infrastructure.Database;

namespace Project.Infrastructure.Processing
{
    /// <summary>
    /// Dispatches domain events collected from tracked entities.
    /// </summary>
    /// <param name="mediator">The mediator used to publish domain events.</param>
    /// <param name="context">The database context from which domain events are collected.</param>
    public class DomainEventsDispatcher(IMediator mediator, ApplicationDbContext context) : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        /// <inheritdoc />
        public async Task DispatchEventsAsync()
        {
            var domainEntities = _context.ChangeTracker
                .Entries<Entity>()
                .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Count != 0)
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(e => e.Entity.DomainEvents!)
                .ToList();

            domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }
        }
    }
}
