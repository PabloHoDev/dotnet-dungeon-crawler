using System;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Combat;

namespace DungeonCrawler.Domain.Enemies.Behaviors
{
    /// <summary>
    /// Two-phase boss. In the first phase it uses its first ability; once its
    /// health drops below the threshold it becomes enraged: every attack hits
    /// harder and it switches to its second ability.
    /// </summary>
    public sealed class BossBehavior : IEnemyBehavior
    {
        private readonly double _enrageBelowHealthFraction;
        private readonly double _enrageMultiplier;

        public BossBehavior(double enrageBelowHealthFraction, double enrageMultiplier)
        {
            if (enrageBelowHealthFraction < 0.0 || enrageBelowHealthFraction > 1.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(enrageBelowHealthFraction),
                    enrageBelowHealthFraction,
                    "Must be between 0 and 1.");
            }

            if (enrageMultiplier < 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(enrageMultiplier), enrageMultiplier, "Must be at least 1.");
            }

            _enrageBelowHealthFraction = enrageBelowHealthFraction;
            _enrageMultiplier = enrageMultiplier;
        }

        public bool IsEnraged(Enemy self)
        {
            Guard.AgainstNull(self, nameof(self));
            return self.Health.Fraction <= _enrageBelowHealthFraction;
        }

        public EnemyIntent Decide(Enemy self, ICombatant target, IRandomSource random)
        {
            var enraged = IsEnraged(self);
            var multiplier = enraged ? _enrageMultiplier : 1.0;

            var abilities = self.Definition.Abilities;
            var phaseAbilityIndex = enraged ? Math.Min(1, abilities.Count - 1) : 0;

            if (abilities.Count > 0 && self.IsReady(abilities[phaseAbilityIndex]))
            {
                return EnemyIntent.Use(abilities[phaseAbilityIndex], multiplier);
            }

            return EnemyIntent.BasicAttack(multiplier);
        }
    }
}
