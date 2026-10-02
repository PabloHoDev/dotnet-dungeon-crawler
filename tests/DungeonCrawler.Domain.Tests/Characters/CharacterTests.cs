using DungeonCrawler.Domain.Abilities;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Items;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.SharedKernel;
using DungeonCrawler.Domain.Tests.TestDoubles;

namespace DungeonCrawler.Domain.Tests.Characters;

public class CharacterCreationTests
{
    [Theory]
    [InlineData(ClassId.Warrior)]
    [InlineData(ClassId.Mage)]
    [InlineData(ClassId.Rogue)]
    public void A_new_character_starts_at_level_1_fully_recovered_with_the_class_setup(ClassId classId)
    {
        var characterClass = CharacterClass.Get(classId);

        var character = Character.Create(Guid.NewGuid(), "Aria", characterClass);

        Assert.Equal(Level.First, character.Level);
        Assert.Equal(0, character.Experience);
        Assert.Equal(characterClass.BaseAttributes, character.BaseAttributes);
        Assert.True(character.Health.IsFull);
        Assert.Equal(character.MaxResource, character.Resource.Current);
        Assert.Equal(0, character.UnspentAttributePoints);
        Assert.Empty(character.Inventory.Items);
        Assert.Empty(character.Loadout.Equipped);
    }

    [Theory]
    [InlineData(ClassId.Warrior)]
    [InlineData(ClassId.Mage)]
    [InlineData(ClassId.Rogue)]
    public void A_new_character_knows_only_the_first_ability_of_its_class(ClassId classId)
    {
        var character = Character.Create(Guid.NewGuid(), "Aria", CharacterClass.Get(classId));

        var ability = Assert.Single(character.Abilities);
        Assert.Same(character.Class.Abilities[0], ability.Definition);
        Assert.Equal(1, ability.Rank);
    }

    [Fact]
    public void Every_class_starts_with_the_same_number_of_attribute_points()
    {
        Assert.All(CharacterClass.All, characterClass => Assert.Equal(25, characterClass.BaseAttributes.Total));
    }

    [Fact]
    public void Classes_differ_in_what_they_are_good_at()
    {
        var warrior = Make.Warrior();
        var mage = Make.Mage();
        var rogue = Make.Rogue();

        // Warrior: the most resilient.
        Assert.True(warrior.MaxHealth > mage.MaxHealth && warrior.MaxHealth > rogue.MaxHealth);
        Assert.True(warrior.Defense > mage.Defense && warrior.Defense > rogue.Defense);

        // Rogue: the fastest, most evasive and most likely to land a critical hit.
        Assert.True(rogue.Speed > warrior.Speed && rogue.Speed > mage.Speed);
        Assert.True(rogue.Evasion.Value > warrior.Evasion.Value);
        Assert.True(rogue.CriticalChance.Value > warrior.CriticalChance.Value);
        Assert.True(rogue.Class.CriticalMultiplier > warrior.Class.CriticalMultiplier);

        // Mage: the strongest opening ability.
        Assert.True(FirstAbilityDamage(mage) > FirstAbilityDamage(warrior));
        Assert.True(FirstAbilityDamage(mage) > FirstAbilityDamage(rogue));
    }

    private static int FirstAbilityDamage(Character character)
    {
        var effect = (DamageAbilityEffect)character.Class.Abilities[0].Effect;
        return effect.RawDamage(character, rank: 1);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void A_character_needs_a_name(string name)
    {
        Assert.Throws<ArgumentException>(() => Character.Create(Guid.NewGuid(), name, CharacterClass.Warrior));
    }

    [Fact]
    public void A_character_needs_an_identity()
    {
        Assert.Throws<ArgumentException>(() => Character.Create(Guid.Empty, "Aria", CharacterClass.Warrior));
    }
}

public class CharacterProgressionTests
{
    [Fact]
    public void Experience_below_the_threshold_does_not_change_the_level()
    {
        var character = Make.Warrior();

        var levelsGained = character.GainExperience(99);

        Assert.Equal(0, levelsGained);
        Assert.Equal(Level.First, character.Level);
        Assert.Equal(99, character.Experience);
    }

    [Fact]
    public void Reaching_the_threshold_levels_up_and_grants_points()
    {
        var character = Make.Warrior();

        var levelsGained = character.GainExperience(100);

        Assert.Equal(1, levelsGained);
        Assert.Equal(Level.Of(2), character.Level);
        Assert.Equal(Balance.AttributePointsPerLevel, character.UnspentAttributePoints);
        Assert.Equal(Balance.AbilityPointsPerLevel, character.UnspentAbilityPoints);
    }

    [Fact]
    public void A_large_reward_can_grant_several_levels_at_once()
    {
        var character = Make.Warrior();

        var levelsGained = character.GainExperience(600);

        Assert.Equal(3, levelsGained);
        Assert.Equal(Level.Of(4), character.Level);
        Assert.Equal(3 * Balance.AttributePointsPerLevel, character.UnspentAttributePoints);
    }

    [Fact]
    public void Leveling_up_raises_maximum_health_and_fully_restores_the_character()
    {
        var character = Make.Warrior();
        var maxHealthBefore = character.MaxHealth;
        character.TakeDamage(50);
        character.SpendResource(10);

        character.GainExperience(100);

        Assert.Equal(maxHealthBefore + Balance.HealthPerLevel, character.MaxHealth);
        Assert.True(character.Health.IsFull);
        Assert.Equal(character.MaxResource, character.Resource.Current);
    }

    [Fact]
    public void Progression_stops_at_level_20()
    {
        var character = Make.Warrior();

        character.GainExperience(1000000);
        var levelsGained = character.GainExperience(1000);

        Assert.Equal(Level.Max, character.Level);
        Assert.Equal(ExperienceCurve.MaxTotal, character.Experience);
        Assert.Equal(0, levelsGained);
        Assert.Equal(19 * Balance.AttributePointsPerLevel, character.UnspentAttributePoints);
    }

    [Fact]
    public void Attribute_points_are_spent_where_the_player_chooses()
    {
        var character = Make.Warrior();
        character.GainExperience(100);

        character.AllocateAttribute(AttributeType.Strength, 3);
        character.AllocateAttribute(AttributeType.Luck, 2);

        Assert.Equal(11, character.BaseAttributes.Strength);
        Assert.Equal(5, character.BaseAttributes.Luck);
        Assert.Equal(0, character.UnspentAttributePoints);
    }

    [Fact]
    public void More_points_than_available_cannot_be_spent()
    {
        var character = Make.Warrior();
        character.GainExperience(100);

        Assert.Throws<DomainException>(() => character.AllocateAttribute(AttributeType.Strength, 6));
        Assert.Throws<ArgumentOutOfRangeException>(() => character.AllocateAttribute(AttributeType.Strength, 0));
        Assert.Equal(8, character.BaseAttributes.Strength);
    }

    [Fact]
    public void Derived_statistics_follow_the_attributes()
    {
        var character = Make.Warrior();
        character.GainExperience(300);
        var before = new { character.MaxHealth, character.Defense, character.BasicAttackDamage, Crit = character.CriticalChance.Value };

        character.AllocateAttribute(AttributeType.Vitality, 4);
        character.AllocateAttribute(AttributeType.Strength, 4);
        character.AllocateAttribute(AttributeType.Luck, 2);

        Assert.Equal(before.MaxHealth + (4 * Balance.HealthPerVitality), character.MaxHealth);
        Assert.Equal(before.Defense + 4, character.Defense);
        Assert.Equal(before.BasicAttackDamage + 6, character.BasicAttackDamage);
        Assert.Equal(before.Crit + 0.02, character.CriticalChance.Value, precision: 10);
    }

    [Fact]
    public void Vitality_gained_while_wounded_keeps_the_wound()
    {
        var character = Make.Warrior();
        character.GainExperience(100);
        character.TakeDamage(30);

        character.AllocateAttribute(AttributeType.Vitality, 1);

        Assert.Equal(character.MaxHealth - 30, character.Health.Current);
    }

    [Fact]
    public void Intelligence_raises_the_mana_of_a_mage_but_not_the_energy_of_a_warrior()
    {
        var mage = Make.Mage();
        var warrior = Make.Warrior();
        mage.GainExperience(100);
        warrior.GainExperience(100);
        var mageBefore = mage.MaxResource;
        var warriorBefore = warrior.MaxResource;

        mage.AllocateAttribute(AttributeType.Intelligence, 5);
        warrior.AllocateAttribute(AttributeType.Intelligence, 5);

        Assert.Equal(mageBefore + 10, mage.MaxResource);
        Assert.Equal(warriorBefore, warrior.MaxResource);
    }
}

public class CharacterAbilityTests
{
    [Fact]
    public void An_ability_point_raises_the_rank_of_a_known_ability()
    {
        var character = Make.Warrior();
        character.GainExperience(100);

        character.ImproveAbility("power-strike");

        Assert.Equal(2, character.FindAbility("power-strike")!.Rank);
        Assert.Equal(0, character.UnspentAbilityPoints);
    }

    [Fact]
    public void An_ability_point_learns_a_new_ability_once_the_level_requirement_is_met()
    {
        var character = Make.Warrior();
        character.GainExperience(ExperienceCurve.TotalToReach(Level.Of(3)));

        character.ImproveAbility("second-wind");

        Assert.Equal(1, character.FindAbility("second-wind")!.Rank);
        Assert.Equal(2, character.Abilities.Count);
    }

    [Fact]
    public void An_ability_cannot_be_learned_before_its_level()
    {
        var character = Make.Warrior();
        character.GainExperience(100);

        Assert.False(character.CanImproveAbility(character.Class.GetAbility("second-wind")));
        Assert.Throws<DomainException>(() => character.ImproveAbility("second-wind"));
        Assert.Equal(1, character.UnspentAbilityPoints);
    }

    [Fact]
    public void Without_ability_points_nothing_can_be_improved()
    {
        var character = Make.Warrior();

        Assert.False(character.CanImproveAbility(character.Class.GetAbility("power-strike")));
        Assert.Throws<DomainException>(() => character.ImproveAbility("power-strike"));
    }

    [Fact]
    public void An_ability_stops_at_rank_3()
    {
        var character = Make.Warrior();
        character.GainExperience(ExperienceCurve.TotalToReach(Level.Of(5)));
        character.ImproveAbility("power-strike");
        character.ImproveAbility("power-strike");

        Assert.Equal(Balance.MaxAbilityRank, character.FindAbility("power-strike")!.Rank);
        Assert.False(character.CanImproveAbility(character.Class.GetAbility("power-strike")));
        Assert.Throws<DomainException>(() => character.ImproveAbility("power-strike"));
        Assert.Equal(2, character.UnspentAbilityPoints);
    }

    [Fact]
    public void A_class_only_has_its_own_abilities()
    {
        var character = Make.Warrior();
        character.GainExperience(100);

        Assert.Throws<DomainException>(() => character.ImproveAbility("fireball"));
    }
}

public class CharacterEquipmentTests
{
    [Fact]
    public void Equipping_moves_the_item_from_the_inventory_to_its_slot()
    {
        var character = Make.Warrior();
        var sword = Make.Weapon(attack: 6);
        character.Store(sword);

        character.Equip(sword.Id);

        Assert.Same(sword, character.Loadout.Get(EquipmentSlot.Weapon));
        Assert.Empty(character.Inventory.Items);
    }

    [Fact]
    public void Equipping_over_another_item_sends_the_old_one_back_to_the_inventory()
    {
        var character = Make.Warrior();
        var oldSword = Make.Weapon(attack: 3);
        var newSword = Make.Weapon(attack: 9);
        character.Store(oldSword);
        character.Store(newSword);
        character.Equip(oldSword.Id);

        character.Equip(newSword.Id);

        Assert.Same(newSword, character.Loadout.Get(EquipmentSlot.Weapon));
        Assert.Same(oldSword, Assert.Single(character.Inventory.Items));
    }

    [Fact]
    public void Unequipping_returns_the_item_to_the_inventory()
    {
        var character = Make.Warrior();
        var sword = Make.Weapon(attack: 6);
        character.Store(sword);
        character.Equip(sword.Id);

        character.Unequip(EquipmentSlot.Weapon);

        Assert.Null(character.Loadout.Get(EquipmentSlot.Weapon));
        Assert.Same(sword, Assert.Single(character.Inventory.Items));
    }

    [Fact]
    public void An_empty_slot_cannot_be_unequipped()
    {
        Assert.Throws<DomainException>(() => Make.Warrior().Unequip(EquipmentSlot.Armor));
    }

    [Fact]
    public void An_item_that_is_not_in_the_inventory_cannot_be_equipped()
    {
        Assert.Throws<DomainException>(() => Make.Warrior().Equip(Make.Weapon(5).Id));
    }

    [Fact]
    public void Unequipping_with_a_full_inventory_is_refused_and_keeps_the_item_equipped()
    {
        var character = Make.Warrior();
        var sword = Make.Weapon(attack: 6);
        character.Store(sword);
        character.Equip(sword.Id);
        while (!character.Inventory.IsFull)
        {
            character.Store(Make.Weapon(1));
        }

        var exception = Assert.Throws<BusinessRuleViolationException>(() => character.Unequip(EquipmentSlot.Weapon));

        Assert.IsType<InventoryMustHaveSpaceRule>(exception.BrokenRule);
        Assert.Same(sword, character.Loadout.Get(EquipmentSlot.Weapon));
    }

    [Fact]
    public void A_weapon_adds_its_attack_to_basic_attacks_and_abilities()
    {
        var character = Make.Warrior();
        var before = character.BasicAttackDamage;
        var sword = Make.Weapon(attack: 6);
        character.Store(sword);

        character.Equip(sword.Id);

        Assert.Equal(6, character.AttackBonus);
        Assert.Equal(before + 6, character.BasicAttackDamage);
    }

    [Fact]
    public void Armor_adds_defense_and_maximum_health_without_healing()
    {
        var character = Make.Warrior();
        var defenseBefore = character.Defense;
        var maxHealthBefore = character.MaxHealth;
        var armor = Make.Armor(defense: 5, maxHealth: 20);
        character.Store(armor);

        character.Equip(armor.Id);

        Assert.Equal(defenseBefore + 5, character.Defense);
        Assert.Equal(maxHealthBefore + 20, character.MaxHealth);
        Assert.Equal(maxHealthBefore, character.Health.Current);
    }

    [Fact]
    public void Removing_armor_lowers_current_health_that_no_longer_fits()
    {
        var character = Make.Warrior();
        var armor = Make.Armor(defense: 5, maxHealth: 20);
        character.Store(armor);
        character.Equip(armor.Id);
        character.RecoverFully();

        character.Unequip(EquipmentSlot.Armor);

        Assert.True(character.Health.IsFull);
        Assert.Equal(character.MaxHealth, character.Health.Current);
    }

    [Fact]
    public void An_accessory_raises_effective_attributes_but_not_base_attributes()
    {
        var character = Make.Warrior();
        var ring = Make.Accessory(AttributeType.Vitality, 3);
        var maxHealthBefore = character.MaxHealth;
        character.Store(ring);

        character.Equip(ring.Id);

        Assert.Equal(8, character.BaseAttributes.Vitality);
        Assert.Equal(11, character.EffectiveAttributes.Vitality);
        Assert.Equal(maxHealthBefore + (3 * Balance.HealthPerVitality), character.MaxHealth);
    }
}
