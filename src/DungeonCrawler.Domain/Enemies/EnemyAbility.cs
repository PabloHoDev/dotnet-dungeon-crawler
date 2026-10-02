using System;
using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Enemies
{
    /// <summary>A special attack of an enemy, usable once every few turns.</summary>
    public sealed class EnemyAbility
    {
        public EnemyAbility(string name, double powerMultiplier, int cooldown, double lifeStealFraction = 0.0)
        {
            if (powerMultiplier <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(powerMultiplier), powerMultiplier, "Must be positive.");
            }

            if (lifeStealFraction < 0.0 || lifeStealFraction > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(lifeStealFraction), lifeStealFraction, "Must be between 0 and 1.");
            }

            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
            PowerMultiplier = powerMultiplier;
            Cooldown = Guard.AgainstNegative(cooldown, nameof(cooldown));
            LifeStealFraction = lifeStealFraction;
        }

        public string Name { get; }

        /// <summary>Multiplier applied to the enemy's attack.</summary>
        public double PowerMultiplier { get; }

        /// <summary>Turns the enemy must wait after using the ability before using it again.</summary>
        public int Cooldown { get; }

        /// <summary>Fraction of the damage dealt that the enemy recovers as health.</summary>
        public double LifeStealFraction { get; }
    }
}
