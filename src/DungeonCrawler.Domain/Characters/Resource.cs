using System;
using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Characters
{
    /// <summary>
    /// The pool a class spends to use abilities (Energy, Mana). Immutable; the
    /// current value always stays between zero and the maximum.
    /// </summary>
    public sealed class Resource : ValueObject
    {
        private Resource(int current, int maximum)
        {
            Current = current;
            Maximum = maximum;
        }

        public int Current { get; }

        public int Maximum { get; }

        public static Resource Full(int maximum)
        {
            return Of(maximum, maximum);
        }

        public static Resource Of(int current, int maximum)
        {
            Guard.AgainstNegative(maximum, nameof(maximum));
            Guard.AgainstOutOfRange(current, 0, maximum, nameof(current));
            return new Resource(current, maximum);
        }

        public bool CanAfford(int cost)
        {
            return cost <= Current;
        }

        /// <exception cref="DomainException">The pool does not hold the requested amount.</exception>
        public Resource Spend(int cost)
        {
            Guard.AgainstNegative(cost, nameof(cost));

            if (!CanAfford(cost))
            {
                throw new DomainException($"Not enough resource: needs {cost}, has {Current}.");
            }

            return new Resource(Current - cost, Maximum);
        }

        public Resource Restore(int amount)
        {
            Guard.AgainstNegative(amount, nameof(amount));
            return new Resource(Current + Math.Min(amount, Maximum - Current), Maximum);
        }

        public Resource Refill()
        {
            return new Resource(Maximum, Maximum);
        }

        public Resource WithMaximum(int maximum)
        {
            Guard.AgainstNegative(maximum, nameof(maximum));
            return new Resource(Math.Min(Current, maximum), maximum);
        }

        public override string ToString()
        {
            return $"{Current}/{Maximum}";
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Current;
            yield return Maximum;
        }
    }
}
