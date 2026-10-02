using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawler.Domain.Abstractions
{
    /// <summary>
    /// Base class for value objects: immutable, without identity, and equal
    /// when all of their components are equal.
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject? left, ValueObject? right)
        {
            return !Equals(left, right);
        }

        public bool Equals(ValueObject? other)
        {
            return other is not null
                && GetType() == other.GetType()
                && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ValueObject);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var component in GetEqualityComponents())
            {
                hash.Add(component);
            }

            return hash.ToHashCode();
        }

        /// <summary>The values that define this object's equality, in a fixed order.</summary>
        protected abstract IEnumerable<object?> GetEqualityComponents();
    }
}
