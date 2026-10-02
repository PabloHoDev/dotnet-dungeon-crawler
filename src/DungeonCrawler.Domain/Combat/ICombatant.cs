using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Combat
{
    /// <summary>
    /// What combat needs to know about a participant. Characters and enemies
    /// share this contract without sharing a base class.
    /// </summary>
    public interface ICombatant
    {
        string Name { get; }

        Health Health { get; }

        bool IsDefeated { get; }

        int Defense { get; }

        int Speed { get; }

        Probability Evasion { get; }

        /// <summary>True while the combatant is defending in the current round.</summary>
        bool IsGuarding { get; }

        void TakeDamage(int amount);

        void Heal(int amount);
    }
}
