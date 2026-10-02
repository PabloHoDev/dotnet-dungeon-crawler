using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Enemies
{
    public enum EnemyIntentKind
    {
        Attack,
        Guard,
    }

    /// <summary>What an enemy decided to do this round.</summary>
    public sealed class EnemyIntent
    {
        private EnemyIntent(EnemyIntentKind kind, string actionName, double powerMultiplier, EnemyAbility? ability)
        {
            Kind = kind;
            ActionName = actionName;
            PowerMultiplier = powerMultiplier;
            Ability = ability;
        }

        public EnemyIntentKind Kind { get; }

        public string ActionName { get; }

        public double PowerMultiplier { get; }

        /// <summary>The special ability being used, or null for a basic attack or a guard.</summary>
        public EnemyAbility? Ability { get; }

        public static EnemyIntent BasicAttack(double powerMultiplier = 1.0)
        {
            return new EnemyIntent(EnemyIntentKind.Attack, "Attack", powerMultiplier, null);
        }

        public static EnemyIntent Use(EnemyAbility ability, double extraMultiplier = 1.0)
        {
            Guard.AgainstNull(ability, nameof(ability));
            return new EnemyIntent(EnemyIntentKind.Attack, ability.Name, ability.PowerMultiplier * extraMultiplier, ability);
        }

        public static EnemyIntent Defend()
        {
            return new EnemyIntent(EnemyIntentKind.Guard, "Guard", 0.0, null);
        }
    }
}
