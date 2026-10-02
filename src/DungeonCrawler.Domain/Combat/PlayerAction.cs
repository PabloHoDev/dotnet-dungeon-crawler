using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Combat
{
    public enum PlayerActionKind
    {
        BasicAttack,
        UseAbility,
        Defend,
    }

    /// <summary>What the player chose to do this round (RF-009).</summary>
    public sealed class PlayerAction
    {
        private PlayerAction(PlayerActionKind kind, string? abilityId)
        {
            Kind = kind;
            AbilityId = abilityId;
        }

        public PlayerActionKind Kind { get; }

        public string? AbilityId { get; }

        public static PlayerAction BasicAttack()
        {
            return new PlayerAction(PlayerActionKind.BasicAttack, null);
        }

        public static PlayerAction UseAbility(string abilityId)
        {
            return new PlayerAction(PlayerActionKind.UseAbility, Guard.AgainstNullOrWhiteSpace(abilityId, nameof(abilityId)));
        }

        /// <summary>Halves the damage taken this round and recovers extra resource.</summary>
        public static PlayerAction Defend()
        {
            return new PlayerAction(PlayerActionKind.Defend, null);
        }
    }
}
