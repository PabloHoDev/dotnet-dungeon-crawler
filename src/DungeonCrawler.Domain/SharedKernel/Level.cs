using System;
using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.SharedKernel
{
    /// <summary>
    /// Level of a character or enemy. The MVP caps progression at level 20 (RF-004).
    /// </summary>
    public sealed class Level : ValueObject, IComparable<Level>
    {
        public const int MinValue = 1;
        public const int MaxValue = 20;

        public static readonly Level First = new Level(MinValue);
        public static readonly Level Max = new Level(MaxValue);

        private Level(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public bool IsMax
        {
            get { return Value == MaxValue; }
        }

        public static Level Of(int value)
        {
            return new Level(Guard.AgainstOutOfRange(value, MinValue, MaxValue, nameof(value)));
        }

        /// <summary>Returns the following level.</summary>
        /// <exception cref="DomainException">The level is already the maximum.</exception>
        public Level Next()
        {
            if (IsMax)
            {
                throw new DomainException($"Level {MaxValue} is the maximum level.");
            }

            return new Level(Value + 1);
        }

        public int CompareTo(Level? other)
        {
            return other is null ? 1 : Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
