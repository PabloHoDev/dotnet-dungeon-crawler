using System;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Combat;

namespace DungeonCrawler.Domain.Enemies.Behaviors
{
    /// <summary>
    /// Endurance: raises its guard at a fixed rhythm and hits hard in between,
    /// prolonging the fight and draining the player's resources.
    /// </summary>
    public sealed class TankBehavior : IEnemyBehavior
    {
        private readonly int _guardEveryTurns;

        public TankBehavior(int guardEveryTurns)
        {
            if (guardEveryTurns < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(guardEveryTurns), guardEveryTurns, "Must be at least 2.");
            }

            _guardEveryTurns = guardEveryTurns;
        }

        public EnemyIntent Decide(Enemy self, ICombatant target, IRandomSource random)
        {
            Guard.AgainstNull(self, nameof(self));

            if ((self.TurnsTaken + 1) % _guardEveryTurns == 0)
            {
                return EnemyIntent.Defend();
            }

            var ability = self.FirstReadyAbility();
            return ability is null ? EnemyIntent.BasicAttack() : EnemyIntent.Use(ability);
        }
    }
}
