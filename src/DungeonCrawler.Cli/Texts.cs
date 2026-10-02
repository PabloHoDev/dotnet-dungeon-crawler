using DungeonCrawler.Domain.Abilities;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Dungeon;
using DungeonCrawler.Domain.Items;

namespace DungeonCrawler.Cli;

/// <summary>
/// Portuguese wording for everything the domain exposes as data. The domain
/// speaks in identifiers; the words shown to the player belong to presentation.
/// </summary>
public static class Texts
{
    private static readonly Dictionary<string, string> Names = new()
    {
        // Classes
        ["Warrior"] = "Guerreiro",
        ["Mage"] = "Arcanista",
        ["Rogue"] = "Ladino",

        // Resources
        ["Energy"] = "Energia",
        ["Mana"] = "Mana",

        // Abilities
        ["Attack"] = "Ataque",
        ["Guard"] = "Defesa",
        ["Power Strike"] = "Golpe Poderoso",
        ["Second Wind"] = "Fôlego Renovado",
        ["Crushing Blow"] = "Golpe Esmagador",
        ["Arcane Bolt"] = "Projétil Arcano",
        ["Mend"] = "Restaurar",
        ["Fireball"] = "Bola de Fogo",
        ["Quick Strike"] = "Ataque Rápido",
        ["Adrenaline"] = "Adrenalina",
        ["Backstab"] = "Golpe Traiçoeiro",

        // Enemies
        ["Goblin"] = "Goblin",
        ["Elite Goblin"] = "Goblin de Elite",
        ["Skeleton"] = "Esqueleto",
        ["Orc"] = "Orc",
        ["Skeleton Knight"] = "Cavaleiro Esqueleto",
        ["Crypt Lord"] = "Senhor da Cripta",
        ["Stab"] = "Estocada",
        ["Bone Arrow"] = "Flecha de Osso",
        ["Smash"] = "Pancada",
        ["Shield Slam"] = "Golpe de Escudo",
        ["Dark Bolt"] = "Raio Sombrio",
        ["Soul Drain"] = "Dreno de Alma",

        // Dungeon
        ["Forgotten Crypt"] = "Cripta Esquecida",

        // Items
        ["Rusty Blade"] = "Lâmina Enferrujada",
        ["Iron Blade"] = "Lâmina de Ferro",
        ["Steel Blade"] = "Lâmina de Aço",
        ["Runed Blade"] = "Lâmina Rúnica",
        ["Dragonfang"] = "Presa de Dragão",
        ["Padded Vest"] = "Colete Acolchoado",
        ["Leather Armor"] = "Armadura de Couro",
        ["Chainmail"] = "Cota de Malha",
        ["Plate Armor"] = "Armadura de Placas",
        ["Aegis Plate"] = "Couraça Égide",
        ["Copper Ring"] = "Anel de Cobre",
        ["Silver Ring"] = "Anel de Prata",
        ["Jade Amulet"] = "Amuleto de Jade",
        ["Arcane Talisman"] = "Talismã Arcano",
        ["Crown of Fortune"] = "Coroa da Fortuna",
    };

    public static string Name(string name)
    {
        return Names.TryGetValue(name, out var translated) ? translated : name;
    }

    public static string AttributeName(AttributeType type) => type switch
    {
        AttributeType.Strength => "Força",
        AttributeType.Vitality => "Vitalidade",
        AttributeType.Agility => "Agilidade",
        AttributeType.Intelligence => "Inteligência",
        AttributeType.Luck => "Sorte",
        _ => type.ToString(),
    };

    public static string AttributeHint(AttributeType type) => type switch
    {
        AttributeType.Strength => "dano do Guerreiro",
        AttributeType.Vitality => "vida e defesa",
        AttributeType.Agility => "velocidade, evasão e dano do Ladino",
        AttributeType.Intelligence => "dano e mana do Arcanista",
        AttributeType.Luck => "chance de crítico e qualidade do loot",
        _ => string.Empty,
    };

    public static string ClassPitch(ClassId id) => id switch
    {
        ClassId.Warrior => "resistente, dano consistente, fácil de jogar",
        ClassId.Mage => "habilidades poderosas, pouca vida, depende de mana",
        ClassId.Rogue => "rápido, críticos altos, frágil",
        _ => string.Empty,
    };

    public static string RarityName(Rarity rarity) => rarity switch
    {
        Rarity.Common => "Comum",
        Rarity.Uncommon => "Incomum",
        Rarity.Rare => "Raro",
        Rarity.Epic => "Épico",
        Rarity.Legendary => "Lendário",
        _ => rarity.ToString(),
    };

    public static string SlotName(EquipmentSlot slot) => slot switch
    {
        EquipmentSlot.Weapon => "Arma",
        EquipmentSlot.Armor => "Armadura",
        EquipmentSlot.Accessory => "Acessório",
        _ => slot.ToString(),
    };

    public static string RoomName(RoomType type) => type switch
    {
        RoomType.Combat => "Sala de combate",
        RoomType.Treasure => "Sala do tesouro",
        RoomType.Elite => "Sala de elite",
        RoomType.MiniBoss => "Covil do mini-chefe",
        RoomType.Boss => "Câmara do chefe",
        _ => type.ToString(),
    };

    public static string ItemLine(Item item)
    {
        var text = $"{Name(item.Name)} [{RarityName(item.Rarity)}]";
        return item is Equipment equipment ? $"{text} — {SlotName(equipment.Slot)}: {Bonuses(equipment)}" : text;
    }

    public static string Bonuses(Equipment equipment)
    {
        var parts = new List<string>();

        if (equipment.Modifiers.Attack > 0)
        {
            parts.Add($"+{equipment.Modifiers.Attack} ataque");
        }

        if (equipment.Modifiers.Defense > 0)
        {
            parts.Add($"+{equipment.Modifiers.Defense} defesa");
        }

        if (equipment.Modifiers.MaxHealth > 0)
        {
            parts.Add($"+{equipment.Modifiers.MaxHealth} vida");
        }

        foreach (var type in Enum.GetValues<AttributeType>())
        {
            var bonus = equipment.AttributeBonus.Get(type);
            if (bonus > 0)
            {
                parts.Add($"+{bonus} {AttributeName(type)}");
            }
        }

        return string.Join(", ", parts);
    }

    public static string AbilityLine(Character character, AbilityDefinition ability, int rank)
    {
        var effect = ability.Effect switch
        {
            DamageAbilityEffect damage => $"{damage.RawDamage(character, rank)} de dano",
            HealAbilityEffect heal => $"cura {heal.Amount(character, rank)}",
            _ => string.Empty,
        };

        return $"{Name(ability.Name)} {Roman(rank)} — {effect}, custa {ability.Cost} de {Name(character.Class.ResourceName)}";
    }

    public static string Roman(int rank) => rank switch
    {
        1 => "I",
        2 => "II",
        3 => "III",
        _ => rank.ToString(),
    };

    public static string LogEntry(CombatLogEntry entry)
    {
        var actor = entry.ActorIsPlayer ? entry.ActorName : Name(entry.ActorName);
        var action = Name(entry.ActionName);

        switch (entry.Kind)
        {
            case CombatLogKind.Guard:
                return $"  {actor} se defende.";

            case CombatLogKind.Heal:
                return $"  {actor} usa {action} e recupera {entry.Amount} de vida.";

            default:
                if (entry.WasEvaded)
                {
                    return $"  {actor} usa {action}, mas o golpe é esquivado!";
                }

                var critical = entry.WasCritical ? " CRÍTICO!" : string.Empty;
                var guarded = entry.WasGuarded ? " (reduzido pela defesa)" : string.Empty;
                return $"  {actor} usa {action}: {entry.Amount} de dano.{critical}{guarded}";
        }
    }
}
