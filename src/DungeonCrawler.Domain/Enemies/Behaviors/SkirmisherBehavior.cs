using System;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Combat;

namespace DungeonCrawler.Domain.Enemies.Behaviors
{
    /// <summary>
    /// Ranged harassment: attacks from afar and, once wounded, falls back
    /// (guards) every other turn to stretch the fight.
    /// </summary>
    public sealed class SkirmisherBehavior : IEnemyBehavior
    {
        private readonly double _retreatBelowHealthFraction;

        public SkirmisherBehavior(double retreatBelowHealthFraction)
        {
            if (retreatBelowHealthFraction < 0.0 || retreatBelowHealthFraction > 1.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(retreatBelowHealthFraction),
                    retreatBelowHealthFraction,
                    "Must be between 0 and 1.");
            }

            _retreatBelowHealthFraction = retreatBelowHealthFraction;
        }

        public EnemyIntent Decide(Enemy self, ICombatant target, IRandomSource random)
        {
            Guard.AgainstNull(self, nameof(self));

            var isWounded = self.Health.Fraction < _retreatBelowHealthFraction;
            if (isWounded && self.TurnsTaken % 2 == 1)
            {
                return EnemyIntent.Defend();
            }

            var ability = self.FirstReadyAbility();
            return ability is null ? EnemyIntent.BasicAttack() : EnemyIntent.Use(ability);
        }
    }
}
