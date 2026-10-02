using System;
using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Items
{
    /// <summary>Anything a character can carry in the inventory.</summary>
    public abstract class Item : Entity<Guid>
    {
        protected Item(Guid id, string name, Rarity rarity)
            : base(id)
        {
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
            Rarity = rarity;
        }

        public string Name { get; }

        public Rarity Rarity { get; }
    }
}
