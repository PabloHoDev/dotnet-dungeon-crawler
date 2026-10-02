using System;
using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Items
{
    /// <summary>
    /// The items a character carries. Space is limited, so storage is a decision
    /// and not just a list (RF-025).
    /// </summary>
    public sealed class Inventory
    {
        private readonly List<Item> _items = new List<Item>();

        public Inventory(int capacity)
        {
            if (capacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Capacity must be at least 1.");
            }

            Capacity = capacity;
        }

        public int Capacity { get; }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool IsFull
        {
            get { return _items.Count >= Capacity; }
        }

        public IReadOnlyList<Item> Items
        {
            get { return _items.AsReadOnly(); }
        }

        /// <exception cref="BusinessRuleViolationException">The inventory is full.</exception>
        public void Add(Item item)
        {
            Guard.AgainstNull(item, nameof(item));

            if (_items.Contains(item))
            {
                throw new DomainException($"'{item.Name}' is already in the inventory.");
            }

            if (IsFull)
            {
                throw new BusinessRuleViolationException(new InventoryMustHaveSpaceRule(this));
            }

            _items.Add(item);
        }

        public Item? Find(Guid itemId)
        {
            return _items.Find(item => item.Id == itemId);
        }

        /// <exception cref="DomainException">The item is not in the inventory.</exception>
        public Item Remove(Guid itemId)
        {
            var item = Find(itemId);
            if (item is null)
            {
                throw new DomainException("The item is not in the inventory.");
            }

            _items.Remove(item);
            return item;
        }
    }
}
