using System;
using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Enemies;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.Rewards;

namespace DungeonCrawler.Domain.Dungeon
{
    public enum ExpeditionStatus
    {
        InProgress,
        Completed,
        Failed,
    }

    /// <summary>
    /// One run through a dungeon (RF-020 to RF-024, RF-035 to RF-039). It holds
    /// the temporary progress — current floor, stage and combat — while the
    /// permanent progress stays in the <see cref="Character"/>. Defeating the
    /// boss completes the expedition; the character's death ends it, keeping
    /// everything the character has earned.
    /// </summary>
    public sealed class Expedition : AggregateRoot<Guid>
    {
        private Expedition(Guid id, Character character, DungeonLayout dungeon)
            : base(id)
        {
            Character = character;
            Dungeon = dungeon;
            Status = ExpeditionStatus.InProgress;
        }

        public Character Character { get; }

        public DungeonLayout Dungeon { get; }

        public ExpeditionStatus Status { get; private set; }

        public int RoomsCleared { get; private set; }

        public CombatEncounter? CurrentCombat { get; private set; }

        public Room? CurrentRoom { get; private set; }

        public Floor CurrentFloor
        {
            get { return Dungeon.Floors[FloorIndex]; }
        }

        /// <summary>Position of the current stage inside the floor, starting at 1.</summary>
        public int StageNumber
        {
            get { return StageIndex + 1; }
        }

        public bool IsInCombat
        {
            get { return CurrentCombat is not null; }
        }

        /// <summary>The rooms the player can enter now; empty during combat or after the end.</summary>
        public IReadOnlyList<Room> AvailableRooms
        {
            get
            {
                return Status == ExpeditionStatus.InProgress && !IsInCombat
                    ? CurrentFloor.Stages[StageIndex].Rooms
                    : Array.Empty<Room>();
            }
        }

        private int FloorIndex { get; set; }

        private int StageIndex { get; set; }

        /// <summary>Starts an expedition. The character enters fully recovered.</summary>
        public static Expedition Start(Character character, DungeonDefinition definition, IRandomSource random)
        {
            Guard.AgainstNull(character, nameof(character));

            var dungeon = DungeonGenerator.Generate(definition, random);
            character.RecoverFully();

            return new Expedition(Guid.NewGuid(), character, dungeon);
        }

        /// <summary>
        /// Enters one of the available rooms. A room with an enemy starts a
        /// combat; a treasure room gives its reward and moves the expedition on.
        /// </summary>
        /// <exception cref="DomainException">The expedition is over, a combat is running, or the room does not exist.</exception>
        public RoomEntry EnterRoom(int roomIndex, IRandomSource random)
        {
            Guard.AgainstNull(random, nameof(random));
            EnsureInProgress();

            if (IsInCombat)
            {
                throw new DomainException("Finish the current combat before entering another room.");
            }

            var rooms = AvailableRooms;
            if (roomIndex < 0 || roomIndex >= rooms.Count)
            {
                throw new DomainException("There is no such room to enter.");
            }

            var room = rooms[roomIndex];
            CurrentRoom = room;

            if (room.HasEnemy)
            {
                var enemy = EnemyFactory.Create(room.Enemy!, room.Modifier, Character.Level, CurrentFloor.Number);
                CurrentCombat = new CombatEncounter(Character, enemy);
                return RoomEntry.Combat(room, CurrentCombat);
            }

            var granted = Reward
                .ForTreasure(CurrentFloor.Number, Character.EffectiveAttributes.Luck, random)
                .GrantTo(Character);

            return RoomEntry.Treasure(room, granted, Advance());
        }

        /// <summary>
        /// Plays one round of the current combat. When the round ends the combat,
        /// the expedition applies the consequence: reward and progress on
        /// victory, the end of the expedition on defeat.
        /// </summary>
        /// <exception cref="DomainException">There is no combat in progress.</exception>
        public TurnOutcome TakeTurn(PlayerAction action, IRandomSource random)
        {
            EnsureInProgress();

            if (CurrentCombat is null)
            {
                throw new DomainException("There is no combat in progress.");
            }

            var combat = CurrentCombat;
            var round = combat.ExecuteRound(action, random);

            switch (round.Status)
            {
                case CombatStatus.Victory:
                    var granted = Reward
                        .ForDefeating(combat.Enemy, CurrentFloor.Number, Character.EffectiveAttributes.Luck, random)
                        .GrantTo(Character);
                    CurrentCombat = null;
                    return new TurnOutcome(round, granted, Advance());

                case CombatStatus.Defeat:
                    CurrentCombat = null;
                    Status = ExpeditionStatus.Failed;

                    // Death ends the expedition but never erases permanent progress.
                    Character.RecoverFully();
                    return new TurnOutcome(round, null, Advancement.ExpeditionFailed);

                default:
                    return new TurnOutcome(round, null, Advancement.None);
            }
        }

        private Advancement Advance()
        {
            RoomsCleared++;
            StageIndex++;

            if (StageIndex < CurrentFloor.Stages.Count)
            {
                return Advancement.NextStage;
            }

            if (FloorIndex == Dungeon.Floors.Count - 1)
            {
                Status = ExpeditionStatus.Completed;
                return Advancement.ExpeditionCompleted;
            }

            FloorIndex++;
            StageIndex = 0;
            Character.Rest(Balance.RestHealFraction);
            return Advancement.NextFloor;
        }

        private void EnsureInProgress()
        {
            if (Status != ExpeditionStatus.InProgress)
            {
                throw new DomainException("The expedition is already over.");
            }
        }
    }
}
