using System;

namespace DungeonCrawler.Domain.Progression
{
    /// <summary>
    /// Mathematical Easter Egg #3 — damage scaling.
    /// Damage = BaseDamage + Attribute × Scaling + AbilityRank × Growth + EquipmentModifier.
    /// </summary>
    public static class DamageFormula
    {
        /// <summary>Damage of an ability before critical hits and the target's defense.</summary>
        public static int Raw(double baseDamage, int attribute, double scaling, int rank, double growth, int equipmentBonus)
        {
            return Round(baseDamage + (attribute * scaling) + (rank * growth) + equipmentBonus);
        }

        public static int Critical(int damage, double multiplier)
        {
            return Round(damage * multiplier);
        }

        /// <summary>
        /// Reduces damage by the target's defense with diminishing returns:
        /// damage × K / (K + defense). Defense never reduces damage to zero.
        /// </summary>
        public static int Mitigate(int damage, int defense)
        {
            var effectiveDefense = Math.Max(0, defense);
            var reduced = damage * (double)Balance.MitigationConstant / (Balance.MitigationConstant + effectiveDefense);
            return Math.Max(Balance.MinimumDamage, Round(reduced));
        }

        public static int Guarded(int damage)
        {
            return Math.Max(Balance.MinimumDamage, Round(damage * Balance.GuardDamageFactor));
        }

        private static int Round(double value)
        {
            return (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }
    }
}
