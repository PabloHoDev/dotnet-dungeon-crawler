using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Combat;

namespace DungeonCrawler.Domain.Enemies.Behaviors
{
    /// <summary>
    /// Melee pressure: always attacks, using the special ability whenever it is ready.
    /// </summary>
    public sealed class AggressiveBehavior : IEnemyBehavior
    {
        public EnemyIntent Decide(Enemy self, ICombatant target, IRandomSource random)
        {
            Guard.AgainstNull(self, nameof(self));

            var ability = self.FirstReadyAbility();
            return ability is null ? EnemyIntent.BasicAttack() : EnemyIntent.Use(ability);
        }
    }
}
