using System;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Enemies
{
    /// <summary>
    /// Builds the enemy of an encounter from a definition, an optional modifier,
    /// the player's level and the floor (RF-016).
    /// </summary>
    public static class EnemyFactory
    {
        public static Enemy Create(
            EnemyDefinition definition,
            EnemyModifier? modifier,
            Level playerLevel,
            int floorNumber)
        {
            Guard.AgainstNull(definition, nameof(definition));

            var scaling = EnemyScaling.Multiplier(playerLevel, floorNumber);
            var floorScaling = EnemyScaling.FloorMultiplier(floorNumber);

            var healthMultiplier = scaling * (modifier?.HealthMultiplier ?? 1.0);
            var attackMultiplier = scaling * (modifier?.AttackMultiplier ?? 1.0);
            var rewardMultiplier = scaling * (modifier?.RewardMultiplier ?? 1.0);

            // Defense grows with depth only: scaling it with the player's level as well
            // would cancel the player's own growth and make fights longer, not harder.
            var defenseMultiplier = floorScaling * (modifier?.DefenseMultiplier ?? 1.0);

            return new Enemy(
                Guid.NewGuid(),
                definition,
                modifier is null ? definition.Name : $"{modifier.Name} {definition.Name}",
                modifier?.ResultingCategory ?? definition.Category,
                Math.Max(1, EnemyScaling.Scale(definition.BaseHealth, healthMultiplier)),
                EnemyScaling.Scale(definition.BaseAttack, attackMultiplier),
                EnemyScaling.Scale(definition.BaseDefense, defenseMultiplier),
                EnemyScaling.Scale(definition.BaseExperience, rewardMultiplier),
                EnemyScaling.Scale(definition.BaseGold, rewardMultiplier));
        }
    }
}
