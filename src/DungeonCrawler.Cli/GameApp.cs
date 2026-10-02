using DungeonCrawler.Application;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Dungeon;
using DungeonCrawler.Domain.Items;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.Rewards;

namespace DungeonCrawler.Cli;

/// <summary>
/// The game flow in the terminal: hub, expedition, combat and character
/// screens. It only reads input, calls the application and domain, and prints
/// the result; it contains no game rules.
/// </summary>
public sealed class GameApp
{
    private readonly GameSession _session;
    private readonly ConsoleUi _ui;

    public GameApp(GameSession session, ConsoleUi ui)
    {
        _session = session;
        _ui = ui;
    }

    private Character Hero => _session.Character!;

    public void Run()
    {
        _ui.Title("DUNGEON CRAWLER");
        _ui.Line("Explore a Cripta Esquecida, derrote o Senhor da Cripta e saia vivo.");

        CreateCharacter();

        while (true)
        {
            _ui.Title($"HUB — {HeroSummary()}");
            _ui.Line("1) Iniciar expedição");
            _ui.Line("2) Personagem");
            _ui.Line("3) Inventário e equipamentos");
            _ui.Line("4) Criar outro personagem");
            _ui.Line("0) Sair");

            switch (_ui.Choose(">", ["1", "2", "3", "4", "0"]))
            {
                case "1":
                    RunExpedition();
                    break;
                case "2":
                    CharacterScreen();
                    break;
                case "3":
                    InventoryScreen();
                    break;
                case "4":
                    CreateCharacter();
                    break;
                default:
                    _ui.Line("Até a próxima expedição!");
                    return;
            }
        }
    }

    // -------------------------------------------------------------------
    // Character creation
    // -------------------------------------------------------------------

    private void CreateCharacter()
    {
        _ui.Section("Criação de personagem");

        var name = _ui.Ask("Nome do personagem:");
        while (string.IsNullOrWhiteSpace(name))
        {
            name = _ui.Ask("O nome não pode ficar vazio. Nome do personagem:");
        }

        _ui.Line();
        var classes = CharacterClass.All;
        for (var i = 0; i < classes.Count; i++)
        {
            _ui.Line($"{i + 1}) {Texts.Name(classes[i].Name)} — {Texts.ClassPitch(classes[i].Id)}");
        }

        var choice = _ui.AskNumber("Classe:", 1, classes.Count);
        _session.CreateCharacter(name, classes[choice - 1].Id);

        _ui.Line();
        _ui.Line($"{Hero.Name}, {Texts.Name(Hero.Class.Name)}, está pronto para a aventura.");
    }

    // -------------------------------------------------------------------
    // Expedition
    // -------------------------------------------------------------------

    private void RunExpedition()
    {
        var expedition = _session.StartExpedition();
        _ui.Title($"EXPEDIÇÃO — {Texts.Name(expedition.Dungeon.Name)}");
        _ui.Line($"{expedition.Dungeon.Floors.Count} andares separam você do chefe.");

        while (expedition.Status == ExpeditionStatus.InProgress)
        {
            var rooms = expedition.AvailableRooms;

            _ui.Section(
                $"Andar {expedition.CurrentFloor.Number}/{expedition.Dungeon.Floors.Count} — " +
                $"trecho {expedition.StageNumber}/{expedition.CurrentFloor.Stages.Count}");
            _ui.Line(Vitals());
            _ui.Line();

            var options = new List<string> { "p", "i" };
            for (var i = 0; i < rooms.Count; i++)
            {
                _ui.Line($"{i + 1}) Entrar: {Texts.RoomName(rooms[i].Type)}");
                options.Add((i + 1).ToString());
            }

            _ui.Line("p) Personagem" + (HasPointsToSpend() ? "  (há pontos para distribuir!)" : string.Empty));
            _ui.Line("i) Inventário e equipamentos");

            var choice = _ui.Choose(">", options);
            if (choice == "p")
            {
                CharacterScreen();
                continue;
            }

            if (choice == "i")
            {
                InventoryScreen();
                continue;
            }

            EnterRoom(int.Parse(choice) - 1);
        }

        if (expedition.Status == ExpeditionStatus.Completed)
        {
            _ui.Title("VITÓRIA!");
            _ui.Line("O Senhor da Cripta foi derrotado. A expedição está concluída!");
        }
        else
        {
            _ui.Title("DERROTA");
            _ui.Line($"{Hero.Name} caiu na cripta. A expedição terminou.");
            _ui.Line("Nível, experiência, atributos, habilidades e itens foram preservados.");
        }

        _ui.Line($"Salas vencidas: {expedition.RoomsCleared} — {HeroSummary()}");
        _ui.Pause();
    }

    private void EnterRoom(int roomIndex)
    {
        var entry = _session.EnterRoom(roomIndex);

        if (entry.StartedCombat is null)
        {
            _ui.Line();
            _ui.Line("Você encontra um baú!");
            ShowReward(entry.Reward!);
            ShowAdvancement(entry.Advancement);
            return;
        }

        RunCombat(entry.StartedCombat);
    }

    private void RunCombat(CombatEncounter combat)
    {
        var enemyName = Texts.Name(combat.Enemy.Name);

        _ui.Section($"COMBATE — {enemyName}");
        _ui.Line(combat.PlayerActsFirst ? "Você é mais rápido e age primeiro." : $"{enemyName} é mais rápido e age primeiro.");

        while (true)
        {
            _ui.Line();
            _ui.Line($"{enemyName,-22} {ConsoleUi.Bar(combat.Enemy.Health.Current, combat.Enemy.Health.Maximum)} {combat.Enemy.Health}");
            _ui.Line($"{Hero.Name,-22} {ConsoleUi.Bar(Hero.Health.Current, Hero.Health.Maximum)} {Hero.Health}   {Texts.Name(Hero.Class.ResourceName)} {Hero.Resource}");

            var outcome = _session.TakeTurn(AskAction());

            _ui.Line();
            foreach (var entry in outcome.Round.Log)
            {
                _ui.Line(Texts.LogEntry(entry));
            }

            if (outcome.Round.Status == CombatStatus.Victory)
            {
                _ui.Line();
                _ui.Line($"{enemyName} foi derrotado!");
                ShowReward(outcome.Reward!);
                ShowAdvancement(outcome.Advancement);
                return;
            }

            if (outcome.Round.Status == CombatStatus.Defeat)
            {
                return;
            }
        }
    }

    private PlayerAction AskAction()
    {
        var actions = new Dictionary<string, PlayerAction>();

        _ui.Line($"1) Atacar ({Hero.BasicAttackDamage} de dano)");
        actions["1"] = PlayerAction.BasicAttack();

        var number = 2;
        foreach (var ability in Hero.Abilities)
        {
            var affordable = Hero.Resource.CanAfford(ability.Definition.Cost);
            var line = Texts.AbilityLine(Hero, ability.Definition, ability.Rank);

            if (affordable)
            {
                _ui.Line($"{number}) {line}");
                actions[number.ToString()] = PlayerAction.UseAbility(ability.Definition.Id);
            }
            else
            {
                _ui.Line($"-) {line} (recurso insuficiente)");
            }

            number++;
        }

        _ui.Line($"d) Defender (metade do dano, +{Hero.Class.ResourceRegeneration} de {Texts.Name(Hero.Class.ResourceName)})");
        actions["d"] = PlayerAction.Defend();

        return actions[_ui.Choose(">", actions.Keys)];
    }

    private void ShowReward(GrantedReward granted)
    {
        var reward = granted.Reward;

        if (reward.Experience > 0)
        {
            _ui.Line($"  +{reward.Experience} XP");
        }

        if (reward.Gold > 0)
        {
            _ui.Line($"  +{reward.Gold} de ouro");
        }

        if (reward.Item is not null)
        {
            _ui.Line(granted.ItemStored
                ? $"  Item encontrado: {Texts.ItemLine(reward.Item)}"
                : $"  Item perdido (inventário cheio): {Texts.ItemLine(reward.Item)}");
        }

        if (granted.LevelsGained > 0)
        {
            _ui.Line();
            _ui.Line($"*** SUBIU DE NÍVEL! Agora nível {Hero.Level}. Vida e recurso restaurados. ***");
            _ui.Line($"    Pontos de atributo: {Hero.UnspentAttributePoints} | Pontos de habilidade: {Hero.UnspentAbilityPoints}");
            _ui.Line("    Use a tela de Personagem (p) para distribuir.");
        }
    }

    private void ShowAdvancement(Advancement advancement)
    {
        if (advancement == Advancement.NextFloor)
        {
            _ui.Line();
            _ui.Line("Você desce para o próximo andar e descansa um pouco pelo caminho.");
        }
    }

    // -------------------------------------------------------------------
    // Character screen
    // -------------------------------------------------------------------

    private void CharacterScreen()
    {
        while (true)
        {
            _ui.Title($"{Hero.Name} — {Texts.Name(Hero.Class.Name)} nível {Hero.Level}");

            _ui.Line(Hero.Level.IsMax
                ? $"XP: {Hero.Experience} (nível máximo)"
                : $"XP: {Hero.Experience} / {ExperienceCurve.TotalToComplete(Hero.Level)} " +
                  $"(faltam {ExperienceCurve.RemainingToNextLevel(Hero.Experience)})");
            _ui.Line($"{Vitals()}   Ouro {Hero.Gold}");

            _ui.Section("Atributos");
            foreach (var type in Enum.GetValues<AttributeType>())
            {
                var bonus = Hero.Loadout.AttributeBonus.Get(type);
                var bonusText = bonus > 0 ? $" (+{bonus} de itens)" : string.Empty;
                _ui.Line($"  {Texts.AttributeName(type),-13} {Hero.BaseAttributes.Get(type),3}{bonusText}");
            }

            _ui.Section("Estatísticas");
            _ui.Line($"  Ataque básico {Hero.BasicAttackDamage} | Defesa {Hero.Defense} | Velocidade {Hero.Speed}");
            _ui.Line($"  Crítico {Hero.CriticalChance} (x{Hero.Class.CriticalMultiplier:0.0}) | Evasão {Hero.Evasion}");

            _ui.Section("Habilidades");
            foreach (var ability in Hero.Abilities)
            {
                _ui.Line($"  {Texts.AbilityLine(Hero, ability.Definition, ability.Rank)}");
            }

            _ui.Line();
            _ui.Line($"1) Distribuir atributos ({Hero.UnspentAttributePoints} pontos)");
            _ui.Line($"2) Aprender ou evoluir habilidades ({Hero.UnspentAbilityPoints} pontos)");
            _ui.Line("0) Voltar");

            switch (_ui.Choose(">", ["1", "2", "0"]))
            {
                case "1":
                    AllocateAttributes();
                    break;
                case "2":
                    ImproveAbilities();
                    break;
                default:
                    return;
            }
        }
    }

    private void AllocateAttributes()
    {
        var types = Enum.GetValues<AttributeType>();

        while (Hero.UnspentAttributePoints > 0)
        {
            _ui.Section($"Distribuir atributos — {Hero.UnspentAttributePoints} pontos disponíveis");
            for (var i = 0; i < types.Length; i++)
            {
                _ui.Line($"{i + 1}) {Texts.AttributeName(types[i]),-13} {Hero.BaseAttributes.Get(types[i]),3}  ({Texts.AttributeHint(types[i])})");
            }

            _ui.Line("0) Voltar");

            var choice = _ui.AskNumber("Atributo:", 0, types.Length);
            if (choice == 0)
            {
                return;
            }

            var points = _ui.AskNumber($"Quantos pontos (1 a {Hero.UnspentAttributePoints})?", 1, Hero.UnspentAttributePoints);
            Hero.AllocateAttribute(types[choice - 1], points);
        }

        _ui.Line("Não há pontos de atributo para distribuir.");
    }

    private void ImproveAbilities()
    {
        while (true)
        {
            _ui.Section($"Habilidades — {Hero.UnspentAbilityPoints} pontos disponíveis");

            var abilities = Hero.Class.Abilities;
            var options = new List<string> { "0" };

            for (var i = 0; i < abilities.Count; i++)
            {
                var learned = Hero.FindAbility(abilities[i].Id);
                var nextRank = learned is null ? 1 : Math.Min(learned.Rank + 1, Balance.MaxAbilityRank);

                string status;
                if (learned is not null && learned.IsMaxRank)
                {
                    status = "no nível máximo";
                }
                else if (Hero.Level.CompareTo(abilities[i].UnlockLevel) < 0)
                {
                    status = $"requer nível {abilities[i].UnlockLevel}";
                }
                else
                {
                    status = learned is null ? "aprender" : $"evoluir para {Texts.Roman(nextRank)}";
                }

                var selectable = Hero.CanImproveAbility(abilities[i]);
                _ui.Line($"{(selectable ? (i + 1).ToString() : "-")}) {Texts.AbilityLine(Hero, abilities[i], nextRank)} [{status}]");

                if (selectable)
                {
                    options.Add((i + 1).ToString());
                }
            }

            _ui.Line("0) Voltar");

            var choice = _ui.Choose(">", options);
            if (choice == "0")
            {
                return;
            }

            Hero.ImproveAbility(abilities[int.Parse(choice) - 1].Id);
        }
    }

    // -------------------------------------------------------------------
    // Inventory screen
    // -------------------------------------------------------------------

    private void InventoryScreen()
    {
        while (true)
        {
            _ui.Title($"Inventário — {Hero.Inventory.Count}/{Hero.Inventory.Capacity}");

            _ui.Section("Equipado");
            foreach (var slot in Enum.GetValues<EquipmentSlot>())
            {
                var equipped = Hero.Loadout.Get(slot);
                _ui.Line($"  {Texts.SlotName(slot),-10} {(equipped is null ? "(vazio)" : Texts.ItemLine(equipped))}");
            }

            _ui.Section("Mochila");
            var items = Hero.Inventory.Items;
            if (items.Count == 0)
            {
                _ui.Line("  (vazia)");
            }

            for (var i = 0; i < items.Count; i++)
            {
                _ui.Line($"  {i + 1}) {Texts.ItemLine(items[i])}");
            }

            _ui.Line();
            _ui.Line("e) Equipar item   r) Remover equipamento   x) Descartar item   0) Voltar");

            try
            {
                switch (_ui.Choose(">", ["e", "r", "x", "0"]))
                {
                    case "e" when items.Count > 0:
                        Hero.Equip(items[_ui.AskNumber("Número do item:", 1, items.Count) - 1].Id);
                        break;

                    case "x" when items.Count > 0:
                        Hero.Discard(items[_ui.AskNumber("Número do item:", 1, items.Count) - 1].Id);
                        break;

                    case "r":
                        var slots = Enum.GetValues<EquipmentSlot>();
                        for (var i = 0; i < slots.Length; i++)
                        {
                            _ui.Line($"  {i + 1}) {Texts.SlotName(slots[i])}");
                        }

                        Hero.Unequip(slots[_ui.AskNumber("Slot:", 1, slots.Length) - 1]);
                        break;

                    case "0":
                        return;

                    default:
                        _ui.Line("A mochila está vazia.");
                        break;
                }
            }
            catch (DomainException exception)
            {
                _ui.Line(RuleMessage(exception));
            }
        }
    }

    // -------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------

    private string HeroSummary()
    {
        return $"{Hero.Name}, {Texts.Name(Hero.Class.Name)} nível {Hero.Level}, {Hero.Gold} de ouro";
    }

    private string Vitals()
    {
        return $"Vida {Hero.Health} {ConsoleUi.Bar(Hero.Health.Current, Hero.Health.Maximum, 12)}   " +
               $"{Texts.Name(Hero.Class.ResourceName)} {Hero.Resource}";
    }

    private bool HasPointsToSpend()
    {
        return Hero.UnspentAttributePoints > 0
            || Hero.Class.Abilities.Any(Hero.CanImproveAbility);
    }

    private static string RuleMessage(DomainException exception)
    {
        return exception is BusinessRuleViolationException
            ? "O inventário está cheio. Descarte um item primeiro."
            : "Não é possível fazer isso agora: o slot está vazio ou o item não pode ser usado.";
    }
}
