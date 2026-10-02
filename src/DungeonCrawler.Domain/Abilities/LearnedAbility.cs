using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Progression;

namespace DungeonCrawler.Domain.Abilities
{
    /// <summary>An ability a character knows, at its current rank (I, II or III).</summary>
    public sealed class LearnedAbility
    {
        public LearnedAbility(AbilityDefinition definition)
        {
            Definition = Guard.AgainstNull(definition, nameof(definition));
            Rank = 1;
        }

        public AbilityDefinition Definition { get; }

        public int Rank { get; private set; }

        public bool IsMaxRank
        {
            get { return Rank >= Balance.MaxAbilityRank; }
        }

        /// <exception cref="DomainException">The ability is already at the maximum rank.</exception>
        public void Upgrade()
        {
            if (IsMaxRank)
            {
                throw new DomainException($"{Definition.Name} is already at the maximum rank.");
            }

            Rank++;
        }
    }
}
