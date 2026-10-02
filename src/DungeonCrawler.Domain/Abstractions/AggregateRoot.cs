using System.Collections.Generic;

namespace DungeonCrawler.Domain.Abstractions
{
    /// <summary>
    /// Entry point of an aggregate: the only object outside code may hold a
    /// reference to, responsible for keeping the whole aggregate consistent.
    /// It records the domain events produced by its operations.
    /// </summary>
    public abstract class AggregateRoot<TId> : Entity<TId>
        where TId : notnull
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        protected AggregateRoot(TId id)
            : base(id)
        {
        }

        /// <summary>Events raised since the last call to <see cref="DequeueDomainEvents"/>.</summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents
        {
            get { return _domainEvents.AsReadOnly(); }
        }

        /// <summary>Returns the pending events and clears them from the aggregate.</summary>
        public IReadOnlyCollection<IDomainEvent> DequeueDomainEvents()
        {
            var events = _domainEvents.ToArray();
            _domainEvents.Clear();
            return events;
        }

        protected void Raise(IDomainEvent domainEvent)
        {
            _domainEvents.Add(Guard.AgainstNull(domainEvent, nameof(domainEvent)));
        }
    }
}
