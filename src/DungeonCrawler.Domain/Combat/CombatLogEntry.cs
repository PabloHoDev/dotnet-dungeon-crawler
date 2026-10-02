using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Combat
{
    public enum CombatLogKind
    {
        Attack,
        Heal,
        Guard,
    }

    /// <summary>
    /// One thing that happened during a round, as data. The presentation layer
    /// decides how to word it.
    /// </summary>
    public sealed class CombatLogEntry
    {
        private CombatLogEntry(
            CombatLogKind kind,
            bool actorIsPlayer,
            string actorName,
            string actionName,
            int amount,
            bool wasCritical,
            bool wasEvaded,
            bool wasGuarded)
        {
            Kind = kind;
            ActorIsPlayer = actorIsPlayer;
            ActorName = actorName;
            ActionName = actionName;
            Amount = amount;
            WasCritical = wasCritical;
            WasEvaded = wasEvaded;
            WasGuarded = wasGuarded;
        }

        public CombatLogKind Kind { get; }

        public bool ActorIsPlayer { get; }

        public string ActorName { get; }

        public string ActionName { get; }

        /// <summary>Damage dealt or health restored.</summary>
        public int Amount { get; }

        public bool WasCritical { get; }

        public bool WasEvaded { get; }

        public bool WasGuarded { get; }

        public static CombatLogEntry Attack(bool actorIsPlayer, string actorName, string actionName, AttackOutcome outcome)
        {
            Guard.AgainstNull(outcome, nameof(outcome));

            return new CombatLogEntry(
                CombatLogKind.Attack,
                actorIsPlayer,
                actorName,
                actionName,
                outcome.Damage,
                outcome.WasCritical,
                outcome.WasEvaded,
                outcome.WasGuarded);
        }

        public static CombatLogEntry Heal(bool actorIsPlayer, string actorName, string actionName, int amount)
        {
            return new CombatLogEntry(CombatLogKind.Heal, actorIsPlayer, actorName, actionName, amount, false, false, false);
        }

        public static CombatLogEntry Guarding(bool actorIsPlayer, string actorName)
        {
            return new CombatLogEntry(CombatLogKind.Guard, actorIsPlayer, actorName, "Guard", 0, false, false, false);
        }
    }
}
