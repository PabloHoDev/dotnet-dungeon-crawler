using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Progression;

namespace DungeonCrawler.Domain.Abilities
{
    /// <summary>Restores the user's health, scaling with an attribute and with the ability's rank.</summary>
    public sealed class HealAbilityEffect : AbilityEffect
    {
        public HealAbilityEffect(double baseAmount, AttributeType scalingAttribute, double scaling, double growthPerRank)
        {
            BaseAmount = baseAmount;
            ScalingAttribute = scalingAttribute;
            Scaling = scaling;
            GrowthPerRank = growthPerRank;
        }

        public double BaseAmount { get; }

        public AttributeType ScalingAttribute { get; }

        public double Scaling { get; }

        public double GrowthPerRank { get; }

        public int Amount(Character user, int rank)
        {
            Guard.AgainstNull(user, nameof(user));

            return DamageFormula.Raw(
                BaseAmount,
                user.EffectiveAttributes.Get(ScalingAttribute),
                Scaling,
                rank,
                GrowthPerRank,
                0);
        }

        public override CombatLogEntry Apply(
            Character user,
            ICombatant target,
            string abilityName,
            int rank,
            IRandomSource random)
        {
            var before = user.Health.Current;
            user.Heal(Amount(user, rank));

            return CombatLogEntry.Heal(true, user.Name, abilityName, user.Health.Current - before);
        }
    }
}
