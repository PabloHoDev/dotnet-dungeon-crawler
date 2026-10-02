using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Abilities
{
    /// <summary>
    /// The static description of an ability: when it unlocks, what it costs and
    /// what it does. Built by composition with an <see cref="AbilityEffect"/>.
    /// </summary>
    public sealed class AbilityDefinition
    {
        public AbilityDefinition(string id, string name, Level unlockLevel, int cost, AbilityEffect effect)
        {
            Id = Guard.AgainstNullOrWhiteSpace(id, nameof(id));
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
            UnlockLevel = Guard.AgainstNull(unlockLevel, nameof(unlockLevel));
            Cost = Guard.AgainstNegative(cost, nameof(cost));
            Effect = Guard.AgainstNull(effect, nameof(effect));
        }

        public string Id { get; }

        public string Name { get; }

        public Level UnlockLevel { get; }

        /// <summary>Resource spent each time the ability is used.</summary>
        public int Cost { get; }

        public AbilityEffect Effect { get; }
    }
}
