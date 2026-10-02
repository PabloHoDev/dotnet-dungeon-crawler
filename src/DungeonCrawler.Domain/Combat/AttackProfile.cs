using System;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Combat
{
    /// <summary>The offensive side of an attack, before the target's defenses.</summary>
    public sealed class AttackProfile
    {
        public AttackProfile(int rawDamage, Probability criticalChance, double criticalMultiplier)
        {
            if (criticalMultiplier < 1.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(criticalMultiplier),
                    criticalMultiplier,
                    "A critical hit cannot deal less damage than a normal hit.");
            }

            RawDamage = Guard.AgainstNegative(rawDamage, nameof(rawDamage));
            CriticalChance = Guard.AgainstNull(criticalChance, nameof(criticalChance));
            CriticalMultiplier = criticalMultiplier;
        }

        public int RawDamage { get; }

        public Probability CriticalChance { get; }

        public double CriticalMultiplier { get; }
    }
}
