using System;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Progression
{
    /// <summary>
    /// Mathematical Easter Egg #2 — derived statistics.
    /// DerivedStat = Base + Attribute × Scaling + Level × Growth.
    /// Derived values are never stored arbitrarily: they are always recomputed
    /// from the character's state.
    /// </summary>
    public static class StatFormulas
    {
        public static int MaxHealth(int baseHealth, int vitality, Level level, int equipmentBonus)
        {
            return baseHealth
                + (vitality * Balance.HealthPerVitality)
                + (level.Value * Balance.HealthPerLevel)
                + equipmentBonus;
        }

        public static int Defense(int vitality, int equipmentBonus)
        {
            return (vitality * Balance.DefensePerVitality) + equipmentBonus;
        }

        public static int MaxResource(int baseResource, int intelligence, int resourcePerIntelligence)
        {
            return baseResource + (intelligence * resourcePerIntelligence);
        }

        /// <summary>Mathematical Easter Egg #5 — limits: the chance never exceeds the cap.</summary>
        public static Probability CriticalChance(Probability baseChance, int luck)
        {
            return Probability
                .Clamp(baseChance.Value + (luck * Balance.CriticalChancePerLuck))
                .CappedAt(Probability.Of(Balance.MaxCriticalChance));
        }

        /// <summary>Mathematical Easter Egg #5 — limits: the chance never exceeds the cap.</summary>
        public static Probability Evasion(int agility)
        {
            return Probability
                .Clamp(agility * Balance.EvasionPerAgility)
                .CappedAt(Probability.Of(Balance.MaxEvasion));
        }

        public static int BasicAttackDamage(int attribute, int equipmentBonus)
        {
            return (int)Math.Round(
                Balance.BasicAttackBaseDamage + (attribute * Balance.BasicAttackScaling) + equipmentBonus,
                MidpointRounding.AwayFromZero);
        }
    }
}
