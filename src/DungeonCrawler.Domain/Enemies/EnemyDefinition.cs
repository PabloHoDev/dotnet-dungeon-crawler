using System;
using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Enemies.Behaviors;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Enemies
{
    /// <summary>
    /// The template of an enemy: base statistics before scaling, its abilities
    /// and the behavior that drives it. An enemy type is a composition of these
    /// parts, never a subclass.
    /// </summary>
    public sealed class EnemyDefinition
    {
        public EnemyDefinition(
            string id,
            string name,
            EnemyCategory category,
            EnemyArchetype archetype,
            int baseHealth,
            int baseAttack,
            int baseDefense,
            int speed,
            Probability criticalChance,
            int baseExperience,
            int baseGold,
            IEnemyBehavior behavior,
            IReadOnlyList<EnemyAbility> abilities)
        {
            if (baseHealth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(baseHealth), baseHealth, "Must be at least 1.");
            }

            Id = Guard.AgainstNullOrWhiteSpace(id, nameof(id));
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
            Category = category;
            Archetype = archetype;
            BaseHealth = baseHealth;
            BaseAttack = Guard.AgainstNegative(baseAttack, nameof(baseAttack));
            BaseDefense = Guard.AgainstNegative(baseDefense, nameof(baseDefense));
            Speed = Guard.AgainstNegative(speed, nameof(speed));
            CriticalChance = Guard.AgainstNull(criticalChance, nameof(criticalChance));
            BaseExperience = Guard.AgainstNegative(baseExperience, nameof(baseExperience));
            BaseGold = Guard.AgainstNegative(baseGold, nameof(baseGold));
            Behavior = Guard.AgainstNull(behavior, nameof(behavior));
            Abilities = Guard.AgainstNull(abilities, nameof(abilities));
        }

        public string Id { get; }

        public string Name { get; }

        public EnemyCategory Category { get; }

        public EnemyArchetype Archetype { get; }

        public int BaseHealth { get; }

        public int BaseAttack { get; }

        public int BaseDefense { get; }

        public int Speed { get; }

        public Probability CriticalChance { get; }

        public int BaseExperience { get; }

        public int BaseGold { get; }

        public IEnemyBehavior Behavior { get; }

        public IReadOnlyList<EnemyAbility> Abilities { get; }
    }
}
