using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Combat;

namespace DungeonCrawler.Domain.Enemies.Behaviors
{
    /// <summary>
    /// Strategy that decides what an enemy does on its turn. Behaviors are
    /// interchangeable and shared between enemies: a new enemy is a new
    /// combination of data and behavior, not a new subclass of <see cref="Enemy"/>.
    /// </summary>
    public interface IEnemyBehavior
    {
        EnemyIntent Decide(Enemy self, ICombatant target, IRandomSource random);
    }
}
