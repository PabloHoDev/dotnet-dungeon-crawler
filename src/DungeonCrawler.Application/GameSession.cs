using System;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Dungeon;

namespace DungeonCrawler.Application
{
    /// <summary>
    /// The use cases of a play session: create the character, start an
    /// expedition, enter rooms and play combat turns. It holds the session state
    /// in memory (Save/Load is outside the MVP) and supplies the domain with the
    /// source of randomness, so presentation layers never deal with either.
    /// </summary>
    public sealed class GameSession
    {
        private readonly IRandomSource _random;

        public GameSession(IRandomSource random)
        {
            _random = Guard.AgainstNull(random, nameof(random));
        }

        /// <summary>The player's character; null until one is created.</summary>
        public Character? Character { get; private set; }

        /// <summary>The most recent expedition, in progress or finished; null before the first one.</summary>
        public Expedition? Expedition { get; private set; }

        public bool HasExpeditionInProgress
        {
            get { return Expedition is not null && Expedition.Status == ExpeditionStatus.InProgress; }
        }

        /// <summary>RF-001, RF-002.</summary>
        public Character CreateCharacter(string name, ClassId classId)
        {
            if (HasExpeditionInProgress)
            {
                throw new DomainException("Cannot create a character during an expedition.");
            }

            Character = Character.Create(Guid.NewGuid(), name, CharacterClass.Get(classId));
            Expedition = null;
            return Character;
        }

        /// <summary>RF-020. Permanent progress carries over from previous expeditions (RF-037).</summary>
        public Expedition StartExpedition()
        {
            if (Character is null)
            {
                throw new DomainException("Create a character before starting an expedition.");
            }

            if (HasExpeditionInProgress)
            {
                throw new DomainException("An expedition is already in progress.");
            }

            Expedition = Expedition.Start(Character, DungeonDefinition.ForgottenCrypt, _random);
            return Expedition;
        }

        /// <summary>RF-022, RF-023.</summary>
        public RoomEntry EnterRoom(int roomIndex)
        {
            return CurrentExpedition().EnterRoom(roomIndex, _random);
        }

        /// <summary>RF-009 to RF-014.</summary>
        public TurnOutcome TakeTurn(PlayerAction action)
        {
            return CurrentExpedition().TakeTurn(action, _random);
        }

        private Expedition CurrentExpedition()
        {
            if (Expedition is null)
            {
                throw new DomainException("No expedition has been started.");
            }

            return Expedition;
        }
    }
}
