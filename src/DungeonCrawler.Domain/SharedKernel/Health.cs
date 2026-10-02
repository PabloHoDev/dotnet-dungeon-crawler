using System;
using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.SharedKernel
{
    /// <summary>
    /// Hit points of a combatant: a current value that always stays between
    /// zero and the maximum. Instances are immutable; every operation returns
    /// a new <see cref="Health"/>.
    /// </summary>
    public sealed class Health : ValueObject
    {
        private Health(int current, int maximum)
        {
            Current = current;
            Maximum = maximum;
        }

        public int Current { get; }

        public int Maximum { get; }

        /// <summary>True when there are no hit points left (the combatant is defeated).</summary>
        public bool IsDepleted
        {
            get { return Current == 0; }
        }

        public bool IsFull
        {
            get { return Current == Maximum; }
        }

        /// <summary>Remaining health as a fraction in the range [0, 1].</summary>
        public double Fraction
        {
            get { return (double)Current / Maximum; }
        }

        public static Health Full(int maximum)
        {
            return Of(maximum, maximum);
        }

        public static Health Of(int current, int maximum)
        {
            if (maximum < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maximum), maximum, "Maximum health must be at least 1.");
            }

            Guard.AgainstOutOfRange(current, 0, maximum, nameof(current));
            return new Health(current, maximum);
        }

        /// <summary>Removes hit points, never going below zero.</summary>
        public Health TakeDamage(int amount)
        {
            Guard.AgainstNegative(amount, nameof(amount));
            return new Health(Math.Max(0, Current - amount), Maximum);
        }

        /// <summary>Restores hit points, never going above the maximum.</summary>
        public Health Heal(int amount)
        {
            Guard.AgainstNegative(amount, nameof(amount));

            // Subtraction form avoids overflow when amount is close to int.MaxValue.
            var missing = Maximum - Current;
            return new Health(Current + Math.Min(amount, missing), Maximum);
        }

        /// <summary>
        /// Changes the maximum (e.g. after a level up or an equipment change),
        /// lowering the current value if it no longer fits.
        /// </summary>
        public Health WithMaximum(int maximum)
        {
            if (maximum < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maximum), maximum, "Maximum health must be at least 1.");
            }

            return new Health(Math.Min(Current, maximum), maximum);
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
