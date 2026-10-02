using System;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Enemies;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Items
{
    /// <summary>
    /// Domain service that creates loot (RF-032 to RF-034). Rarity comes from
    /// <see cref="LootProbability"/>; the power of the item grows with the floor
    /// and with the rarity.
    /// </summary>
    public static class LootGenerator
    {
        private static readonly double[] RarityPower = { 1.0, 1.4, 1.9, 2.6, 3.5 };

        private static readonly string[] WeaponNames =
            { "Rusty Blade", "Iron Blade", "Steel Blade", "Runed Blade", "Dragonfang" };

        private static readonly string[] ArmorNames =
            { "Padded Vest", "Leather Armor", "Chainmail", "Plate Armor", "Aegis Plate" };

        private static readonly string[] AccessoryNames =
            { "Copper Ring", "Silver Ring", "Jade Amulet", "Arcane Talisman", "Crown of Fortune" };

        private static readonly AttributeType[] AccessoryAttributes =
        {
            AttributeType.Strength,
            AttributeType.Vitality,
            AttributeType.Agility,
            AttributeType.Intelligence,
            AttributeType.Luck,
        };

        /// <summary>Chance that an enemy of the given category drops an item.</summary>
        public static Probability DropChance(EnemyCategory category)
        {
            return category == EnemyCategory.Common
                ? Probability.Of(Balance.CommonDropChance)
                : Probability.Always;
        }

        /// <summary>The worst rarity an enemy of the given category can drop.</summary>
        public static Rarity MinimumRarity(EnemyCategory category)
        {
            switch (category)
            {
                case EnemyCategory.Elite: return Rarity.Uncommon;
                case EnemyCategory.MiniBoss: return Rarity.Rare;
                case EnemyCategory.Boss: return Rarity.Epic;
                default: return Rarity.Common;
            }
        }

        /// <summary>Rolls the drop of a defeated enemy; null when nothing drops.</summary>
        public static Equipment? RollDrop(EnemyCategory category, int floorNumber, int luck, IRandomSource random)
        {
            Guard.AgainstNull(random, nameof(random));

            return DropChance(category).Roll(random)
                ? Generate(floorNumber, luck, MinimumRarity(category), random)
                : null;
        }

        /// <summary>Creates one piece of equipment for the given floor.</summary>
        public static Equipment Generate(int floorNumber, int luck, Rarity minimumRarity, IRandomSource random)
        {
            Guard.AgainstNull(random, nameof(random));

            if (floorNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(floorNumber), floorNumber, "Floors start at 1.");
            }

            var rarity = LootProbability.Pick(random.NextDouble(), luck);
            if (rarity < minimumRarity)
            {
                rarity = minimumRarity;
            }

            var slot = (EquipmentSlot)random.NextInt(0, 3);
            var power = (2.0 + (floorNumber * 1.5)) * RarityPower[(int)rarity];

            switch (slot)
            {
                case EquipmentSlot.Weapon:
                    return new Equipment(
                        Guid.NewGuid(),
                        WeaponNames[(int)rarity],
                        rarity,
                        slot,
                        Attributes.None,
                        new StatModifiers(Round(power * 1.5), 0, 0));

                case EquipmentSlot.Armor:
                    return new Equipment(
                        Guid.NewGuid(),
                        ArmorNames[(int)rarity],
                        rarity,
                        slot,
                        Attributes.None,
                        new StatModifiers(0, Round(power), Round(power * 4.0)));

                default:
                    var attribute = AccessoryAttributes[random.NextInt(0, AccessoryAttributes.Length)];
                    return new Equipment(
                        Guid.NewGuid(),
                        AccessoryNames[(int)rarity],
                        rarity,
                        slot,
                        Attributes.Of(attribute, Round(power * 0.8)),
                        StatModifiers.None);
            }
        }

        private static int Round(double value)
        {
            return (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }
    }
}
