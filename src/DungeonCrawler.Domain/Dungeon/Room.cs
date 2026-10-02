using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Enemies;

namespace DungeonCrawler.Domain.Dungeon
{
    public enum RoomType
    {
        Combat,
        Treasure,
        Elite,
        MiniBoss,
        Boss,
    }

    /// <summary>
    /// One room of a floor. Combat rooms know which enemy waits inside; the enemy
    /// itself is only created, scaled to the player, when the room is entered.
    /// </summary>
    public sealed class Room
    {
        private Room(RoomType type, EnemyDefinition? enemy, EnemyModifier? modifier)
        {
            Type = type;
            Enemy = enemy;
            Modifier = modifier;
        }

        public RoomType Type { get; }

        public EnemyDefinition? Enemy { get; }

        public EnemyModifier? Modifier { get; }

        public bool HasEnemy
        {
            get { return Enemy is not null; }
        }

        public static Room Treasure()
        {
            return new Room(RoomType.Treasure, null, null);
        }

        public static Room WithEnemy(RoomType type, EnemyDefinition enemy, EnemyModifier? modifier = null)
        {
            Guard.AgainstNull(enemy, nameof(enemy));

            if (type == RoomType.Treasure)
            {
                throw new DomainException("A treasure room has no enemy.");
            }

            return new Room(type, enemy, modifier);
        }
    }
}
