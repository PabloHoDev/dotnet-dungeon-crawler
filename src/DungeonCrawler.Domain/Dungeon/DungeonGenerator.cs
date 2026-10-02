using System;
using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Enemies;

namespace DungeonCrawler.Domain.Dungeon
{
    /// <summary>
    /// Builds the dungeon of one expedition from a definition. The structure is
    /// always the one of the definition; what changes between expeditions is
    /// which enemy occupies each room.
    /// </summary>
    public static class DungeonGenerator
    {
        public static DungeonLayout Generate(DungeonDefinition definition, IRandomSource random)
        {
            Guard.AgainstNull(definition, nameof(definition));
            Guard.AgainstNull(random, nameof(random));

            var floors = new List<Floor>();

            for (var floorIndex = 0; floorIndex < definition.Floors.Count; floorIndex++)
            {
                var template = definition.Floors[floorIndex];
                var stages = new List<Stage>();

                foreach (var roomTypes in template.Stages)
                {
                    var rooms = new List<Room>();
                    foreach (var roomType in roomTypes)
                    {
                        rooms.Add(CreateRoom(roomType, definition, template, random));
                    }

                    stages.Add(new Stage(rooms));
                }

                floors.Add(new Floor(floorIndex + 1, stages));
            }

            return new DungeonLayout(definition.Name, floors);
        }

        private static Room CreateRoom(
            RoomType type,
            DungeonDefinition definition,
            FloorTemplate floor,
            IRandomSource random)
        {
            switch (type)
            {
                case RoomType.Treasure:
                    return Room.Treasure();

                case RoomType.Combat:
                    return Room.WithEnemy(type, Pick(floor.CommonEnemies, random));

                case RoomType.Elite:
                    return Room.WithEnemy(type, Pick(definition.EliteEnemies, random), EnemyModifier.Elite);

                case RoomType.MiniBoss:
                    return Room.WithEnemy(type, definition.MiniBoss);

                case RoomType.Boss:
                    return Room.WithEnemy(type, definition.Boss);

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown room type.");
            }
        }

        private static EnemyDefinition Pick(IReadOnlyList<EnemyDefinition> pool, IRandomSource random)
        {
            return pool[random.NextInt(0, pool.Count)];
        }
    }
}
