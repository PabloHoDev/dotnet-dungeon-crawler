using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Rewards;

namespace DungeonCrawler.Domain.Dungeon
{
    /// <summary>How the expedition moved after a room was resolved.</summary>
    public enum Advancement
    {
        /// <summary>The combat is still running.</summary>
        None,
        NextStage,
        NextFloor,
        ExpeditionCompleted,
        ExpeditionFailed,
    }

    /// <summary>What happened when the player entered a room.</summary>
    public sealed class RoomEntry
    {
        private RoomEntry(Room room, CombatEncounter? combat, GrantedReward? reward, Advancement advancement)
        {
            Room = room;
            StartedCombat = combat;
            Reward = reward;
            Advancement = advancement;
        }

        public Room Room { get; }

        /// <summary>The combat that started, or null for a room without an enemy.</summary>
        public CombatEncounter? StartedCombat { get; }

        public GrantedReward? Reward { get; }

        public Advancement Advancement { get; }

        internal static RoomEntry Combat(Room room, CombatEncounter combat)
        {
            return new RoomEntry(room, combat, null, Advancement.None);
        }

        internal static RoomEntry Treasure(Room room, GrantedReward reward, Advancement advancement)
        {
            return new RoomEntry(room, null, reward, advancement);
        }
    }

    /// <summary>What happened in one combat round and its consequence for the expedition.</summary>
    public sealed class TurnOutcome
    {
        public TurnOutcome(RoundResult round, GrantedReward? reward, Advancement advancement)
        {
            Round = round;
            Reward = reward;
            Advancement = advancement;
        }

        public RoundResult Round { get; }

        /// <summary>The reward of the combat, when the round ended it in victory.</summary>
        public GrantedReward? Reward { get; }

        public Advancement Advancement { get; }
    }
}
