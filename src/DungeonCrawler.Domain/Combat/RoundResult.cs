using System.Collections.Generic;

namespace DungeonCrawler.Domain.Combat
{
    public enum CombatStatus
    {
        InProgress,
        Victory,
        Defeat,
    }

    /// <summary>Everything that happened in one round, in order, and where the combat stands.</summary>
    public sealed class RoundResult
    {
        public RoundResult(int round, IReadOnlyList<CombatLogEntry> log, CombatStatus status)
        {
            Round = round;
            Log = log;
            Status = status;
        }

        public int Round { get; }

        public IReadOnlyList<CombatLogEntry> Log { get; }

        public CombatStatus Status { get; }
    }
}
