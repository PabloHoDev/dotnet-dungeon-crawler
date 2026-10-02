using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Combat;

namespace DungeonCrawler.Domain.Abilities
{
    /// <summary>
    /// What an ability does when used. New kinds of abilities are added by
    /// creating a new effect, without touching <see cref="Character"/> or combat.
    /// </summary>
    public abstract class AbilityEffect
    {
        public abstract CombatLogEntry Apply(
            Character user,
            ICombatant target,
            string abilityName,
            int rank,
            IRandomSource random);
    }
}
