using System;
using System.Collections.Generic;

namespace DungeonCrawler.Domain.Abstractions
{
    /// <summary>
    /// Base class for entities: objects defined by their identity rather than
    /// by their current state.
    /// </summary>
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
        where TId : notnull
    {
        protected Entity(TId id)
        {
            if (EqualityComparer<TId>.Default.Equals(id, default!))
            {
                throw new ArgumentException("An entity requires a non-default identity.", nameof(id));
            }

            Id = id;
        }

        public TId Id { get; }

        public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        {
            return !Equals(left, right);
        }

        public bool Equals(Entity<TId>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return GetType() == other.GetType()
                && EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Entity<TId>);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GetType(), Id);
        }

        /// <summary>
        /// Throws <see cref="BusinessRuleViolationException"/> when the rule is broken.
        /// </summary>
        protected static void CheckRule(IBusinessRule rule)
        {
            Guard.AgainstNull(rule, nameof(rule));

            if (rule.IsBroken())
            {
                throw new BusinessRuleViolationException(rule);
            }
        }
    }
}
