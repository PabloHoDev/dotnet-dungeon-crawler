using System;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Dungeon
{
    /// <summary>
    /// A point of the floor where the player picks one room among the available
    /// paths. A stage with a single room is a corridor; with two, a fork.
    /// </summary>
    public sealed class Stage
    {
        public Stage(IReadOnlyList<Room> rooms)
        {
            Guard.AgainstNull(rooms, nameof(rooms));

            if (rooms.Count == 0)
            {
                throw new DomainException("A stage needs at least one room.");
            }

            Rooms = rooms;
        }

        public IReadOnlyList<Room> Rooms { get; }
    }

    public sealed class Floor
    {
        public Floor(int number, IReadOnlyList<Stage> stages)
        {
            Guard.AgainstNull(stages, nameof(stages));

            if (number < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(number), number, "Floors start at 1.");
            }

            if (stages.Count == 0)
            {
                throw new DomainException("A floor needs at least one stage.");
            }

            Number = number;
            Stages = stages;
        }

        public int Number { get; }

        public IReadOnlyList<Stage> Stages { get; }
    }

    /// <summary>
    /// The dungeon of one expedition, as produced by <see cref="DungeonGenerator"/>.
    /// The constructor enforces the generation rules: there is always a path to
    /// the boss, and the boss is the single room of the last stage of the last floor.
    /// </summary>
    public sealed class DungeonLayout
    {
        public DungeonLayout(string name, IReadOnlyList<Floor> floors)
        {
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
            Guard.AgainstNull(floors, nameof(floors));

            if (floors.Count == 0)
            {
                throw new DomainException("A dungeon needs at least one floor.");
            }

            for (var i = 0; i < floors.Count; i++)
            {
                if (floors[i].Number != i + 1)
                {
                    throw new DomainException("Floors must be numbered consecutively from 1.");
                }
            }

            var allRooms = floors.SelectMany(floor => floor.Stages).SelectMany(stage => stage.Rooms).ToList();
            var finalStage = floors[floors.Count - 1].Stages.Last();

            if (allRooms.Count(room => room.Type == RoomType.Boss) != 1
                || finalStage.Rooms.Count != 1
                || finalStage.Rooms[0].Type != RoomType.Boss)
            {
                throw new DomainException("A dungeon must end in exactly one boss room.");
            }

            Floors = floors;
        }

        public string Name { get; }

        public IReadOnlyList<Floor> Floors { get; }
    }
}
