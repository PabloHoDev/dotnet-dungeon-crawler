using DungeonCrawler.Domain.Enemies.Behaviors;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Enemies
{
    /// <summary>
    /// The enemies of the MVP (RF-015). Few enemies, each with a different
    /// behavior, rather than many variants of the same one.
    /// </summary>
    public static class Bestiary
    {
        public static readonly EnemyDefinition Goblin = new EnemyDefinition(
            "goblin",
            "Goblin",
            EnemyCategory.Common,
            EnemyArchetype.Melee,
            baseHealth: 45,
            baseAttack: 12,
            baseDefense: 3,
            speed: 6,
            criticalChance: Probability.Of(0.05),
            baseExperience: 50,
            baseGold: 8,
            behavior: new AggressiveBehavior(),
            abilities: new[] { new EnemyAbility("Stab", 1.3, cooldown: 3) });

        public static readonly EnemyDefinition Skeleton = new EnemyDefinition(
            "skeleton",
            "Skeleton",
            EnemyCategory.Common,
            EnemyArchetype.Ranged,
            baseHealth: 40,
            baseAttack: 15,
            baseDefense: 2,
            speed: 5,
            criticalChance: Probability.Of(0.10),
            baseExperience: 55,
            baseGold: 12,
            behavior: new SkirmisherBehavior(retreatBelowHealthFraction: 0.5),
            abilities: new[] { new EnemyAbility("Bone Arrow", 1.5, cooldown: 2) });

        public static readonly EnemyDefinition Orc = new EnemyDefinition(
            "orc",
            "Orc",
            EnemyCategory.Common,
            EnemyArchetype.Melee,
            baseHealth: 80,
            baseAttack: 16,
            baseDefense: 6,
            speed: 3,
            criticalChance: Probability.Of(0.05),
            baseExperience: 75,
            baseGold: 18,
            behavior: new AggressiveBehavior(),
            abilities: new[] { new EnemyAbility("Smash", 1.8, cooldown: 3) });

        public static readonly EnemyDefinition SkeletonKnight = new EnemyDefinition(
            "skeleton-knight",
            "Skeleton Knight",
            EnemyCategory.MiniBoss,
            EnemyArchetype.Tank,
            baseHealth: 130,
            baseAttack: 17,
            baseDefense: 10,
            speed: 3,
            criticalChance: Probability.Of(0.05),
            baseExperience: 250,
            baseGold: 80,
            behavior: new TankBehavior(guardEveryTurns: 3),
            abilities: new[] { new EnemyAbility("Shield Slam", 1.6, cooldown: 3) });

        public static readonly EnemyDefinition CryptLord = new EnemyDefinition(
            "crypt-lord",
            "Crypt Lord",
            EnemyCategory.Boss,
            EnemyArchetype.Caster,
            baseHealth: 200,
            baseAttack: 17,
            baseDefense: 9,
            speed: 6,
            criticalChance: Probability.Of(0.10),
            baseExperience: 500,
            baseGold: 200,
            behavior: new BossBehavior(enrageBelowHealthFraction: 0.5, enrageMultiplier: 1.25),
            abilities: new[]
            {
                new EnemyAbility("Dark Bolt", 1.6, cooldown: 3),
                new EnemyAbility("Soul Drain", 1.4, cooldown: 3, lifeStealFraction: 0.35),
            });
    }
}
