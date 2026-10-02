using System;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;

namespace DungeonCrawler.Domain.Items
{
    /// <summary>
    /// An item that occupies an equipment slot and improves the character
    /// through attribute bonuses and stat modifiers.
    /// </summary>
    public sealed class Equipment : Item
    {
        public Equipment(
            Guid id,
            string name,
            Rarity rarity,
            EquipmentSlot slot,
            Attributes attributeBonus,
            StatModifiers modifiers)
            : base(id, name, rarity)
        {
            Slot = slot;
            AttributeBonus = Guard.AgainstNull(attributeBonus, nameof(attributeBonus));
            Modifiers = Guard.AgainstNull(modifiers, nameof(modifiers));
        }

        public EquipmentSlot Slot { get; }

        public Attributes AttributeBonus { get; }

        public StatModifiers Modifiers { get; }
    }
}
