using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Enemies;

namespace DungeonCrawler.Domain.Dungeon
{
    /// <summary>The fixed part of a floor: its sequence of stages and the enemies that may appear.</summary>
    public sealed class FloorTemplate
    {
        public FloorTemplate(IReadOnlyList<IReadOnlyList<RoomType>> stages, IReadOnlyList<EnemyDefinition> commonEnemies)
        {
            Stages = Guard.AgainstNull(stages, nameof(stages));
            CommonEnemies = Guard.AgainstNull(commonEnemies, nameof(commonEnemies));

            if (commonEnemies.Count == 0)
            {
                throw new DomainException("A floor needs at least one common enemy.");
            }
        }

        /// <summary>For each stage, the type of each room the player can choose from.</summary>
        public IReadOnlyList<IReadOnlyList<RoomType>> Stages { get; }

        public IReadOnlyList<EnemyDefinition> CommonEnemies { get; }
    }

    /// <summary>
    /// The hand-authored structure of a dungeon — the "fixed" half of the hybrid
    /// model: name, floors, stage sequence, enemy pools, mini boss and boss.
    /// The variable half (which enemy is in which room) is decided per expedition
    /// by <see cref="DungeonGenerator"/>.
    /// </summary>
    public sealed class DungeonDefinition
    {
        private static readonly RoomType[] Corridor = { RoomType.Combat };
        private static readonly RoomType[] Fork = { RoomType.Combat, RoomType.Treasure };

        /// <summary>The dungeon of the MVP: five floors ending in the Crypt Lord (RF-021).</summary>
        public static readonly DungeonDefinition ForgottenCrypt = new DungeonDefinition(
            "Forgotten Crypt",
            new[]
            {
                new FloorTemplate(
                    new[] { Corridor, Fork, Corridor },
                    new[] { Bestiary.Goblin }),
                new FloorTemplate(
                    new[] { Corridor, Fork, Corridor },
                    new[] { Bestiary.Goblin, Bestiary.Skeleton }),
                new FloorTemplate(
                    new[] { Corridor, Fork, new[] { RoomType.Elite } },
                    new[] { Bestiary.Skeleton, Bestiary.Orc }),
                new FloorTemplate(
                    new[] { Corridor, Fork, new[] { RoomType.MiniBoss } },
                    new[] { Bestiary.Skeleton, Bestiary.Orc }),
                new FloorTemplate(
                    new[] { Corridor, Fork, new[] { RoomType.Boss } },
                    new[] { Bestiary.Orc }),
            },
            eliteEnemies: new[] { Bestiary.Goblin },
            miniBoss: Bestiary.SkeletonKnight,
            boss: Bestiary.CryptLord);

        public DungeonDefinition(
            string name,
            IReadOnlyList<FloorTemplate> floors,
            IReadOnlyList<EnemyDefinition> eliteEnemies,
            EnemyDefinition miniBoss,
            EnemyDefinition boss)
        {
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
            Floors = Guard.AgainstNull(floors, nameof(floors));
            EliteEnemies = Guard.AgainstNull(eliteEnemies, nameof(eliteEnemies));
            MiniBoss = Guard.AgainstNull(miniBoss, nameof(miniBoss));
            Boss = Guard.AgainstNull(boss, nameof(boss));

            if (eliteEnemies.Count == 0)
            {
                throw new DomainException("A dungeon needs at least one enemy that can appear as elite.");
            }
        }

        public string Name { get; }

        public IReadOnlyList<FloorTemplate> Floors { get; }

        /// <summary>Common enemies that receive the Elite modifier in elite rooms.</summary>
        public IReadOnlyList<EnemyDefinition> EliteEnemies { get; }

        public EnemyDefinition MiniBoss { get; }

        public EnemyDefinition Boss { get; }
    }
}
