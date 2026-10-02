using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;

namespace DungeonCrawler.Domain.Items
{
    /// <summary>
    /// What a character is currently wearing: at most one item per slot.
    /// Kept apart from the <see cref="Inventory"/>, which only stores items.
    /// </summary>
    public sealed class Loadout
    {
        private readonly Dictionary<EquipmentSlot, Equipment> _equipped = new Dictionary<EquipmentSlot, Equipment>();

        public IReadOnlyCollection<Equipment> Equipped
        {
            get { return _equipped.Values; }
        }

        /// <summary>Sum of the attribute bonuses of everything equipped.</summary>
        public Attributes AttributeBonus
        {
            get
            {
                var total = Attributes.None;
                foreach (var equipment in _equipped.Values)
                {
                    total = total.Plus(equipment.AttributeBonus);
                }

                return total;
            }
        }

        /// <summary>Sum of the stat modifiers of everything equipped.</summary>
        public StatModifiers Modifiers
        {
            get
            {
                var total = StatModifiers.None;
                foreach (var equipment in _equipped.Values)
                {
                    total = total.Plus(equipment.Modifiers);
                }

                return total;
            }
        }

        public Equipment? Get(EquipmentSlot slot)
        {
            return _equipped.TryGetValue(slot, out var equipment) ? equipment : null;
        }

        /// <summary>Puts the item in its slot and returns what was there before, if anything.</summary>
        public Equipment? Equip(Equipment equipment)
        {
            Guard.AgainstNull(equipment, nameof(equipment));

            var previous = Get(equipment.Slot);
            _equipped[equipment.Slot] = equipment;
            return previous;
        }

        /// <exception cref="DomainException">The slot is empty.</exception>
        public Equipment Unequip(EquipmentSlot slot)
        {
            var equipment = Get(slot);
            if (equipment is null)
            {
                throw new DomainException($"Nothing is equipped in the {slot} slot.");
            }

            _equipped.Remove(slot);
            return equipment;
        }
    }
}
