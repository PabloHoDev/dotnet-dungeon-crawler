using System;
using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Enemies
{
    /// <summary>
    /// Turns an existing enemy into a stronger variant without creating a new
    /// type: Goblin + Elite = Elite Goblin (RF-017).
    /// </summary>
    public sealed class EnemyModifier
    {
        public static readonly EnemyModifier Elite = new EnemyModifier(
            "Elite",
            EnemyCategory.Elite,
            healthMultiplier: 1.6,
            attackMultiplier: 1.3,
            defenseMultiplier: 1.3,
            rewardMultiplier: 2.5);

        public EnemyModifier(
            string name,
            EnemyCategory resultingCategory,
            double healthMultiplier,
            double attackMultiplier,
            double defenseMultiplier,
            double rewardMultiplier)
        {
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
            ResultingCategory = resultingCategory;
            HealthMultiplier = Positive(healthMultiplier, nameof(healthMultiplier));
            AttackMultiplier = Positive(attackMultiplier, nameof(attackMultiplier));
            DefenseMultiplier = Positive(defenseMultiplier, nameof(defenseMultiplier));
            RewardMultiplier = Positive(rewardMultiplier, nameof(rewardMultiplier));
        }

        public string Name { get; }

        public EnemyCategory ResultingCategory { get; }

        public double HealthMultiplier { get; }

        public double AttackMultiplier { get; }

        public double DefenseMultiplier { get; }

        public double RewardMultiplier { get; }

        private static double Positive(double value, string paramName)
        {
            if (double.IsNaN(value) || value <= 0.0)
            {
                throw new ArgumentOutOfRangeException(paramName, value, "Must be positive.");
            }

            return value;
        }
    }
}
