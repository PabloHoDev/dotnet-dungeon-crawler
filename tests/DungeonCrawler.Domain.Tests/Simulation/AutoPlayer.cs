using DungeonCrawler.Domain.Abilities;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Dungeon;
using DungeonCrawler.Domain.Items;

namespace DungeonCrawler.Domain.Tests.Simulation;

/// <summary>
/// A simple, sensible player used to drive whole expeditions in tests: it
/// spends its points, wears the best gear it finds, heals when hurt and
/// otherwise uses its strongest affordable attack.
/// </summary>
public static class AutoPlayer
{
    public static ExpeditionStatus PlayExpedition(Character character, IRandomSource random)
    {
        var expedition = Expedition.Start(character, DungeonDefinition.ForgottenCrypt, random);
        return Play(expedition, random);
    }

    public static ExpeditionStatus Play(Expedition expedition, IRandomSource random)
    {
        var character = expedition.Character;

        while (expedition.Status == ExpeditionStatus.InProgress)
        {
            Prepare(character);

            var entry = expedition.EnterRoom(ChooseRoom(expedition), random);
            if (entry.StartedCombat is null)
            {
                continue;
            }

            while (expedition.IsInCombat)
            {
                expedition.TakeTurn(ChooseAction(character), random);
            }
        }

        return expedition.Status;
    }

    public static void Prepare(Character character)
    {
        SpendAttributePoints(character);
        SpendAbilityPoints(character);
        EquipBestItems(character);
    }

    private static int ChooseRoom(Expedition expedition)
    {
        var rooms = expedition.AvailableRooms;
        if (rooms.Count == 1)
        {
            return 0;
        }

        // Hurt: take the safe room. Healthy: fight for the experience.
        var preferTreasure = expedition.Character.Health.Fraction < 0.6;
        for (var i = 0; i < rooms.Count; i++)
        {
            if ((rooms[i].Type == RoomType.Treasure) == preferTreasure)
            {
                return i;
            }
        }

        return 0;
    }

    private static PlayerAction ChooseAction(Character character)
    {
        LearnedAbility? bestDamage = null;
        var bestDamageValue = 0;
        LearnedAbility? heal = null;

        foreach (var ability in character.Abilities)
        {
            if (!character.Resource.CanAfford(ability.Definition.Cost))
            {
                continue;
            }

            if (ability.Definition.Effect is DamageAbilityEffect damage)
            {
                var value = damage.RawDamage(character, ability.Rank);
                if (value > bestDamageValue)
                {
                    bestDamage = ability;
                    bestDamageValue = value;
                }
            }
            else if (ability.Definition.Effect is HealAbilityEffect)
            {
                heal = ability;
            }
        }

        if (heal is not null && character.Health.Fraction < 0.45)
        {
            return PlayerAction.UseAbility(heal.Definition.Id);
        }

        return bestDamage is not null && bestDamageValue > character.BasicAttackDamage
            ? PlayerAction.UseAbility(bestDamage.Definition.Id)
            : PlayerAction.BasicAttack();
    }

    private static void SpendAttributePoints(Character character)
    {
        // Three points in the class attribute for every two in Vitality.
        while (character.UnspentAttributePoints > 0)
        {
            var primary = Math.Min(3, character.UnspentAttributePoints);
            character.AllocateAttribute(character.Class.PrimaryAttribute, primary);

            var vitality = Math.Min(2, character.UnspentAttributePoints);
            if (vitality > 0)
            {
                character.AllocateAttribute(AttributeType.Vitality, vitality);
            }
        }
    }

    private static void SpendAbilityPoints(Character character)
    {
        while (true)
        {
            // New abilities first, then ranks, in class order.
            var next = character.Class.Abilities
                .Where(character.CanImproveAbility)
                .OrderBy(ability => character.FindAbility(ability.Id) is null ? 0 : 1)
                .FirstOrDefault();

            if (next is null)
            {
                return;
            }

            character.ImproveAbility(next.Id);
        }
    }

    private static void EquipBestItems(Character character)
    {
        foreach (var item in character.Inventory.Items.OfType<Equipment>().ToList())
        {
            var equipped = character.Loadout.Get(item.Slot);
            if (equipped is null || Score(item) > Score(equipped))
            {
                character.Equip(item.Id);
            }
        }

        // Keep room for new loot: drop what is not being worn.
        foreach (var item in character.Inventory.Items.ToList())
        {
            character.Discard(item.Id);
        }
    }

    private static int Score(Equipment equipment)
    {
        return (equipment.Modifiers.Attack * 3)
            + (equipment.Modifiers.Defense * 3)
            + equipment.Modifiers.MaxHealth
            + (equipment.AttributeBonus.Total * 4);
    }
}
