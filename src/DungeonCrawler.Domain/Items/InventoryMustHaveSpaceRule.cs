using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Items
{
    public sealed class InventoryMustHaveSpaceRule : IBusinessRule
    {
        private readonly Inventory _inventory;

        public InventoryMustHaveSpaceRule(Inventory inventory)
        {
            _inventory = Guard.AgainstNull(inventory, nameof(inventory));
        }

        public string Message
        {
            get { return $"The inventory is full ({_inventory.Capacity} items)."; }
        }

        public bool IsBroken()
        {
            return _inventory.IsFull;
        }
    }
}
