using System;
using System.Collections.Generic;
using System.Globalization;
using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.SharedKernel
{
    /// <summary>
    /// A chance between 0 (never) and 1 (always), used by critical hits, loot and
    /// encounters. Keeping the range inside the type is what guarantees the
    /// "limits" rule of the progression design: no chance can grow past 100%.
    /// </summary>
    public sealed class Probability : ValueObject, IComparable<Probability>
    {
        public static readonly Probability Never = new Probability(0.0);
        public static readonly Probability Always = new Probability(1.0);

        private Probability(double value)
        {
            Value = value;
        }

        /// <summary>The chance as a fraction in the range [0, 1].</summary>
        public double Value { get; }

        /// <summary>Creates a probability from a fraction in the range [0, 1].</summary>
        public static Probability Of(double value)
        {
            if (double.IsNaN(value) || value < 0.0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "A probability must be between 0 and 1.");
            }

            return new Probability(value);
        }

        /// <summary>Creates a probability from a percentage in the range [0, 100].</summary>
        public static Probability FromPercent(double percent)
        {
            if (double.IsNaN(percent) || percent < 0.0 || percent > 100.0)
            {
                throw new ArgumentOutOfRangeException(nameof(percent), percent, "A percentage must be between 0 and 100.");
            }

            return new Probability(percent / 100.0);
        }

        /// <summary>
        /// Creates a probability from the result of a formula, clamping it to [0, 1]
        /// instead of rejecting it.
        /// </summary>
        public static Probability Clamp(double value)
        {
            if (double.IsNaN(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "A probability cannot be NaN.");
            }

            return new Probability(Math.Max(0.0, Math.Min(1.0, value)));
        }

        /// <summary>Returns this probability limited to <paramref name="maximum"/>.</summary>
        public Probability CappedAt(Probability maximum)
        {
            Guard.AgainstNull(maximum, nameof(maximum));
            return Value > maximum.Value ? maximum : this;
        }

        /// <summary>
        /// Rolls against this probability. A chance of 0 never succeeds and a
        /// chance of 1 always succeeds, because the roll is in [0, 1).
        /// </summary>
        public bool Roll(IRandomSource random)
        {
            Guard.AgainstNull(random, nameof(random));
            return random.NextDouble() < Value;
        }

        public int CompareTo(Probability? other)
        {
            return other is null ? 1 : Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return (Value * 100.0).ToString("0.##", CultureInfo.InvariantCulture) + "%";
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
