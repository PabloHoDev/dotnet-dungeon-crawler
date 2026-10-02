using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Enemies;
using DungeonCrawler.Domain.Items;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Tests.TestDoubles;

/// <summary>Short builders for the objects most tests need.</summary>
public static class Make
{
    public static Character Warrior(string name = "Aria")
    {
        return Character.Create(Guid.NewGuid(), name, CharacterClass.Warrior);
    }

    public static Character Mage(string name = "Merin")
    {
        return Character.Create(Guid.NewGuid(), name, CharacterClass.Mage);
    }

    public static Character Rogue(string name = "Vex")
    {
        return Character.Create(Guid.NewGuid(), name, CharacterClass.Rogue);
    }

    public static Equipment Weapon(int attack, Rarity rarity = Rarity.Common)
    {
        return new Equipment(Guid.NewGuid(), "Test Blade", rarity, EquipmentSlot.Weapon, Attributes.None, new StatModifiers(attack, 0, 0));
    }

    public static Equipment Armor(int defense, int maxHealth)
    {
        return new Equipment(Guid.NewGuid(), "Test Armor", Rarity.Common, EquipmentSlot.Armor, Attributes.None, new StatModifiers(0, defense, maxHealth));
    }

    public static Equipment Accessory(AttributeType attribute, int points)
    {
        return new Equipment(Guid.NewGuid(), "Test Ring", Rarity.Common, EquipmentSlot.Accessory, Attributes.Of(attribute, points), StatModifiers.None);
    }

    /// <summary>An enemy at base power: level 1 player, floor 1, no modifier.</summary>
    public static Enemy Enemy(EnemyDefinition definition)
    {
        return EnemyFactory.Create(definition, null, Level.First, 1);
    }
}
