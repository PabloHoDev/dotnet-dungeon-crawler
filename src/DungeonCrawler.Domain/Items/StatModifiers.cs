using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Items
{
    /// <summary>Flat bonuses an item adds to derived statistics (RF-031).</summary>
    public sealed class StatModifiers : ValueObject
    {
        public static readonly StatModifiers None = new StatModifiers(0, 0, 0);

        public StatModifiers(int attack, int defense, int maxHealth)
        {
            Attack = Guard.AgainstNegative(attack, nameof(attack));
            Defense = Guard.AgainstNegative(defense, nameof(defense));
            MaxHealth = Guard.AgainstNegative(maxHealth, nameof(maxHealth));
        }

        public int Attack { get; }

        public int Defense { get; }

        public int MaxHealth { get; }

        public StatModifiers Plus(StatModifiers other)
        {
            Guard.AgainstNull(other, nameof(other));
            return new StatModifiers(Attack + other.Attack, Defense + other.Defense, MaxHealth + other.MaxHealth);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Attack;
            yield return Defense;
            yield return MaxHealth;
        }
    }
}
