using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Enemies;
using DungeonCrawler.Domain.Items;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.Tests.TestDoubles;

namespace DungeonCrawler.Domain.Tests.Items;

public class InventoryTests
{
    [Fact]
    public void Items_are_stored_in_the_order_they_arrive()
    {
        var inventory = new Inventory(3);
        var first = Make.Weapon(1);
        var second = Make.Weapon(2);

        inventory.Add(first);
        inventory.Add(second);

        Assert.Equal(new Item[] { first, second }, inventory.Items);
        Assert.Equal(2, inventory.Count);
    }

    [Fact]
    public void A_full_inventory_refuses_new_items()
    {
        var inventory = new Inventory(2);
        inventory.Add(Make.Weapon(1));
        inventory.Add(Make.Weapon(2));

        var exception = Assert.Throws<BusinessRuleViolationException>(() => inventory.Add(Make.Weapon(3)));

        Assert.True(inventory.IsFull);
        Assert.IsType<InventoryMustHaveSpaceRule>(exception.BrokenRule);
        Assert.Equal(2, inventory.Count);
    }

    [Fact]
    public void The_same_item_cannot_be_stored_twice()
    {
        var inventory = new Inventory(3);
        var sword = Make.Weapon(1);
        inventory.Add(sword);

        Assert.Throws<DomainException>(() => inventory.Add(sword));
    }

    [Fact]
    public void Removing_frees_space_and_returns_the_item()
    {
        var inventory = new Inventory(1);
        var sword = Make.Weapon(1);
        inventory.Add(sword);

        var removed = inventory.Remove(sword.Id);

        Assert.Same(sword, removed);
        Assert.False(inventory.IsFull);
        Assert.Null(inventory.Find(sword.Id));
    }

    [Fact]
    public void An_item_that_is_not_there_cannot_be_removed()
    {
        Assert.Throws<DomainException>(() => new Inventory(1).Remove(Guid.NewGuid()));
    }

    [Fact]
    public void Capacity_must_be_positive()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Inventory(0));
    }
}

public class LootGeneratorTests
{
    [Fact]
    public void A_common_enemy_drops_only_when_the_roll_is_below_the_drop_chance()
    {
        var justBelow = new FixedRandomSource(Balance.CommonDropChance - 0.01, 0.0, 0.0);
        var justAbove = new FixedRandomSource(Balance.CommonDropChance + 0.01);

        Assert.NotNull(LootGenerator.RollDrop(EnemyCategory.Common, 1, 0, justBelow));
        Assert.Null(LootGenerator.RollDrop(EnemyCategory.Common, 1, 0, justAbove));
    }

    [Theory]
    [InlineData(EnemyCategory.Elite, Rarity.Uncommon)]
    [InlineData(EnemyCategory.MiniBoss, Rarity.Rare)]
    [InlineData(EnemyCategory.Boss, Rarity.Epic)]
    public void Stronger_enemies_always_drop_and_never_below_their_minimum_rarity(EnemyCategory category, Rarity minimum)
    {
        // Worst possible rolls: the drop roll is as high as it gets and the rarity roll lands on Common.
        var worstLuck = new FixedRandomSource(0.99, 0.0, 0.0);

        var drop = LootGenerator.RollDrop(category, 1, 0, worstLuck);

        Assert.NotNull(drop);
        Assert.Equal(minimum, drop.Rarity);
    }

    [Fact]
    public void A_great_roll_is_not_lowered_to_the_minimum()
    {
        var item = LootGenerator.Generate(1, 0, Rarity.Uncommon, new FixedRandomSource(0.995, 0.0));

        Assert.Equal(Rarity.Legendary, item.Rarity);
    }

    [Fact]
    public void Each_slot_improves_a_different_part_of_the_character()
    {
        var weapon = LootGenerator.Generate(1, 0, Rarity.Common, new FixedRandomSource(0.0, 0.0));
        var armor = LootGenerator.Generate(1, 0, Rarity.Common, new FixedRandomSource(0.0, 0.4));
        var accessory = LootGenerator.Generate(1, 0, Rarity.Common, new FixedRandomSource(0.0, 0.8, 0.0));

        Assert.Equal(EquipmentSlot.Weapon, weapon.Slot);
        Assert.True(weapon.Modifiers.Attack > 0);

        Assert.Equal(EquipmentSlot.Armor, armor.Slot);
        Assert.True(armor.Modifiers.Defense > 0 && armor.Modifiers.MaxHealth > 0);

        Assert.Equal(EquipmentSlot.Accessory, accessory.Slot);
        Assert.True(accessory.AttributeBonus.Total > 0);
    }

    [Fact]
    public void Deeper_floors_and_better_rarities_give_stronger_items()
    {
        var shallow = LootGenerator.Generate(1, 0, Rarity.Common, new FixedRandomSource(0.0, 0.0));
        var deep = LootGenerator.Generate(5, 0, Rarity.Common, new FixedRandomSource(0.0, 0.0));
        var rare = LootGenerator.Generate(1, 0, Rarity.Rare, new FixedRandomSource(0.0, 0.0));

        Assert.True(deep.Modifiers.Attack > shallow.Modifiers.Attack);
        Assert.True(rare.Modifiers.Attack > shallow.Modifiers.Attack);
    }

    [Fact]
    public void The_same_seed_generates_the_same_item()
    {
        var first = LootGenerator.Generate(3, 5, Rarity.Common, new SeededRandomSource(42));
        var second = LootGenerator.Generate(3, 5, Rarity.Common, new SeededRandomSource(42));

        Assert.Equal(first.Name, second.Name);
        Assert.Equal(first.Rarity, second.Rarity);
        Assert.Equal(first.Slot, second.Slot);
        Assert.Equal(first.Modifiers, second.Modifiers);
        Assert.Equal(first.AttributeBonus, second.AttributeBonus);
    }
}
