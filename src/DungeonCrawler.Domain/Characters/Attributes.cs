using System;
using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Characters
{
    /// <summary>
    /// An immutable set of the five base attributes. Used both for a character's
    /// own attributes and for the bonuses granted by equipment.
    /// </summary>
    public sealed class Attributes : ValueObject
    {
        public static readonly Attributes None = new Attributes(0, 0, 0, 0, 0);

        public Attributes(int strength, int vitality, int agility, int intelligence, int luck)
        {
            Strength = Guard.AgainstNegative(strength, nameof(strength));
            Vitality = Guard.AgainstNegative(vitality, nameof(vitality));
            Agility = Guard.AgainstNegative(agility, nameof(agility));
            Intelligence = Guard.AgainstNegative(intelligence, nameof(intelligence));
            Luck = Guard.AgainstNegative(luck, nameof(luck));
        }

        public int Strength { get; }

        public int Vitality { get; }

        public int Agility { get; }

        public int Intelligence { get; }

        public int Luck { get; }

        public int Total
        {
            get { return Strength + Vitality + Agility + Intelligence + Luck; }
        }

        /// <summary>A set with points in a single attribute.</summary>
        public static Attributes Of(AttributeType type, int points)
        {
            return None.Increase(type, points);
        }

        public int Get(AttributeType type)
        {
            switch (type)
            {
                case AttributeType.Strength: return Strength;
                case AttributeType.Vitality: return Vitality;
                case AttributeType.Agility: return Agility;
                case AttributeType.Intelligence: return Intelligence;
                case AttributeType.Luck: return Luck;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown attribute.");
            }
        }

        public Attributes Increase(AttributeType type, int points)
        {
            Guard.AgainstNegative(points, nameof(points));

            switch (type)
            {
                case AttributeType.Strength: return new Attributes(Strength + points, Vitality, Agility, Intelligence, Luck);
                case AttributeType.Vitality: return new Attributes(Strength, Vitality + points, Agility, Intelligence, Luck);
                case AttributeType.Agility: return new Attributes(Strength, Vitality, Agility + points, Intelligence, Luck);
                case AttributeType.Intelligence: return new Attributes(Strength, Vitality, Agility, Intelligence + points, Luck);
                case AttributeType.Luck: return new Attributes(Strength, Vitality, Agility, Intelligence, Luck + points);
                default: throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown attribute.");
            }
        }

        public Attributes Plus(Attributes other)
        {
            Guard.AgainstNull(other, nameof(other));

            return new Attributes(
                Strength + other.Strength,
                Vitality + other.Vitality,
                Agility + other.Agility,
                Intelligence + other.Intelligence,
                Luck + other.Luck);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Strength;
            yield return Vitality;
            yield return Agility;
            yield return Intelligence;
            yield return Luck;
        }
    }
}
