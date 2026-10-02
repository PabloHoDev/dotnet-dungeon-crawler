using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Abilities
{
    /// <summary>Deals damage that scales with an attribute and with the ability's rank.</summary>
    public sealed class DamageAbilityEffect : AbilityEffect
    {
        public DamageAbilityEffect(
            double baseDamage,
            AttributeType scalingAttribute,
            double scaling,
            double growthPerRank,
            double criticalChanceBonus = 0.0)
        {
            BaseDamage = baseDamage;
            ScalingAttribute = scalingAttribute;
            Scaling = scaling;
            GrowthPerRank = growthPerRank;
            CriticalChanceBonus = criticalChanceBonus;
        }

        public double BaseDamage { get; }

        public AttributeType ScalingAttribute { get; }

        public double Scaling { get; }

        public double GrowthPerRank { get; }

        /// <summary>Extra critical chance of this ability, as a fraction added to the user's own.</summary>
        public double CriticalChanceBonus { get; }

        public int RawDamage(Character user, int rank)
        {
            Guard.AgainstNull(user, nameof(user));

            return DamageFormula.Raw(
                BaseDamage,
                user.EffectiveAttributes.Get(ScalingAttribute),
                Scaling,
                rank,
                GrowthPerRank,
                user.AttackBonus);
        }

        public override CombatLogEntry Apply(
            Character user,
            ICombatant target,
            string abilityName,
            int rank,
            IRandomSource random)
        {
            var criticalChance = Probability
                .Clamp(user.CriticalChance.Value + CriticalChanceBonus)
                .CappedAt(Probability.Of(Balance.MaxCriticalChance));

            var attack = new AttackProfile(RawDamage(user, rank), criticalChance, user.Class.CriticalMultiplier);
            var outcome = AttackResolver.Resolve(attack, target, random);

            return CombatLogEntry.Attack(true, user.Name, abilityName, outcome);
        }
    }
}
