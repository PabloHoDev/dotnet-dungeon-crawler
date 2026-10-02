using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Dungeon;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.SharedKernel;
using DungeonCrawler.Domain.Tests.Simulation;
using DungeonCrawler.Domain.Tests.TestDoubles;

namespace DungeonCrawler.Domain.Tests.Expeditions;

public class ExpeditionTests
{
    private static readonly IRandomSource Plain = ConstantRandomSource.NothingHappens;

    private static Expedition Start(Character character)
    {
        return Expedition.Start(character, DungeonDefinition.ForgottenCrypt, new SeededRandomSource(1));
    }

    /// <summary>A character strong enough to win every fight of the dungeon.</summary>
    private static Character Veteran()
    {
        var character = Make.Warrior();
        character.GainExperience(ExperienceCurve.MaxTotal);
        AutoPlayer.Prepare(character);
        return character;
    }

    private static TurnOutcome FightUntilTheCombatEnds(Expedition expedition, IRandomSource random)
    {
        TurnOutcome outcome;
        do
        {
            outcome = expedition.TakeTurn(PlayerAction.BasicAttack(), random);
        }
        while (outcome.Round.Status == CombatStatus.InProgress);

        return outcome;
    }

    [Fact]
    public void An_expedition_starts_on_floor_1_with_the_character_recovered()
    {
        var character = Make.Warrior();
        character.TakeDamage(50);

        var expedition = Start(character);

        Assert.Equal(ExpeditionStatus.InProgress, expedition.Status);
        Assert.Equal(1, expedition.CurrentFloor.Number);
        Assert.Equal(1, expedition.StageNumber);
        Assert.True(character.Health.IsFull);
        Assert.Single(expedition.AvailableRooms);
    }

    [Fact]
    public void Entering_a_room_with_an_enemy_starts_a_combat()
    {
        var expedition = Start(Make.Warrior());

        var entry = expedition.EnterRoom(0, Plain);

        Assert.NotNull(entry.StartedCombat);
        Assert.Same(entry.StartedCombat, expedition.CurrentCombat);
        Assert.True(expedition.IsInCombat);
        Assert.Empty(expedition.AvailableRooms);
    }

    [Fact]
    public void Another_room_cannot_be_entered_during_a_combat()
    {
        var expedition = Start(Make.Warrior());
        expedition.EnterRoom(0, Plain);

        Assert.Throws<DomainException>(() => expedition.EnterRoom(0, Plain));
    }

    [Fact]
    public void A_room_that_does_not_exist_cannot_be_entered()
    {
        var expedition = Start(Make.Warrior());

        Assert.Throws<DomainException>(() => expedition.EnterRoom(5, Plain));
        Assert.Throws<DomainException>(() => expedition.EnterRoom(-1, Plain));
    }

    [Fact]
    public void A_turn_cannot_be_taken_outside_combat()
    {
        var expedition = Start(Make.Warrior());

        Assert.Throws<DomainException>(() => expedition.TakeTurn(PlayerAction.BasicAttack(), Plain));
    }

    [Fact]
    public void Winning_a_combat_grants_the_reward_and_moves_to_the_next_stage()
    {
        var character = Make.Warrior();
        var expedition = Start(character);
        var enemy = expedition.EnterRoom(0, Plain).StartedCombat!.Enemy;

        var outcome = FightUntilTheCombatEnds(expedition, Plain);

        Assert.Equal(CombatStatus.Victory, outcome.Round.Status);
        Assert.Equal(Advancement.NextStage, outcome.Advancement);
        Assert.Equal(enemy.ExperienceReward, outcome.Reward!.Reward.Experience);
        Assert.Equal(enemy.ExperienceReward, character.Experience);
        Assert.Equal(enemy.GoldReward, character.Gold);
        Assert.False(expedition.IsInCombat);
        Assert.Equal(2, expedition.StageNumber);
        Assert.Equal(1, expedition.RoomsCleared);
    }

    [Fact]
    public void The_second_stage_offers_a_choice_between_fighting_and_treasure()
    {
        var expedition = Start(Make.Warrior());
        expedition.EnterRoom(0, Plain);
        FightUntilTheCombatEnds(expedition, Plain);

        Assert.Equal(
            new[] { RoomType.Combat, RoomType.Treasure },
            expedition.AvailableRooms.Select(room => room.Type));
    }

    [Fact]
    public void A_treasure_room_gives_gold_and_an_item_without_a_fight()
    {
        var character = Make.Warrior();
        var expedition = Start(character);
        expedition.EnterRoom(0, Plain);
        FightUntilTheCombatEnds(expedition, Plain);
        var goldBefore = character.Gold;

        var entry = expedition.EnterRoom(1, new SeededRandomSource(5));

        Assert.Null(entry.StartedCombat);
        Assert.Equal(RoomType.Treasure, entry.Room.Type);
        Assert.Equal(goldBefore + Balance.TreasureGoldPerFloor, character.Gold);
        Assert.Single(character.Inventory.Items);
        Assert.True(entry.Reward!.ItemStored);
        Assert.Equal(Advancement.NextStage, entry.Advancement);
        Assert.Equal(3, expedition.StageNumber);
    }

    [Fact]
    public void Clearing_the_last_stage_of_a_floor_leads_to_the_next_floor_after_a_rest()
    {
        var character = Veteran();
        var expedition = Start(character);

        Advancement last;
        do
        {
            expedition.EnterRoom(0, Plain);
            last = FightUntilTheCombatEnds(expedition, Plain).Advancement;
        }
        while (last == Advancement.NextStage);

        Assert.Equal(Advancement.NextFloor, last);
        Assert.Equal(2, expedition.CurrentFloor.Number);
        Assert.Equal(1, expedition.StageNumber);
        Assert.Equal(character.MaxResource, character.Resource.Current);
    }

    [Fact]
    public void Resting_between_floors_restores_part_of_the_health()
    {
        var character = Make.Warrior();
        character.TakeDamage(100);
        var before = character.Health.Current;

        character.Rest(Balance.RestHealFraction);

        Assert.Equal(before + (int)Math.Round(character.MaxHealth * Balance.RestHealFraction), character.Health.Current);
    }

    [Fact]
    public void Defeating_the_boss_completes_the_expedition()
    {
        var expedition = Start(Veteran());

        var status = AutoPlayer.Play(expedition, new SeededRandomSource(1));

        Assert.Equal(ExpeditionStatus.Completed, status);
        Assert.Equal(15, expedition.RoomsCleared);
        Assert.Equal(5, expedition.CurrentFloor.Number);
        Assert.Empty(expedition.AvailableRooms);
    }

    [Fact]
    public void The_death_of_the_character_ends_the_expedition()
    {
        var character = Make.Mage();
        var expedition = Start(character);
        expedition.EnterRoom(0, Plain);
        character.TakeDamage(character.Health.Current - 1);

        var outcome = expedition.TakeTurn(PlayerAction.Defend(), Plain);

        Assert.Equal(CombatStatus.Defeat, outcome.Round.Status);
        Assert.Equal(Advancement.ExpeditionFailed, outcome.Advancement);
        Assert.Equal(ExpeditionStatus.Failed, expedition.Status);
        Assert.Null(outcome.Reward);
        Assert.False(expedition.IsInCombat);
    }

    [Fact]
    public void Death_preserves_the_permanent_progress_of_the_character()
    {
        var character = Make.Warrior();
        var expedition = Start(character);

        // Win the first fight to earn something, then die in the second.
        expedition.EnterRoom(0, Plain);
        FightUntilTheCombatEnds(expedition, Plain);
        character.GainExperience(100);
        character.AllocateAttribute(AttributeType.Strength, 5);
        character.ImproveAbility("power-strike");
        var sword = Make.Weapon(7);
        character.Store(sword);
        character.Equip(sword.Id);

        var snapshot = new
        {
            character.Level,
            character.Experience,
            character.BaseAttributes,
            character.Gold,
            Rank = character.FindAbility("power-strike")!.Rank,
        };

        expedition.EnterRoom(0, Plain);
        character.TakeDamage(character.Health.Current - 1);
        expedition.TakeTurn(PlayerAction.BasicAttack(), Plain);

        Assert.Equal(ExpeditionStatus.Failed, expedition.Status);
        Assert.Equal(snapshot.Level, character.Level);
        Assert.Equal(snapshot.Experience, character.Experience);
        Assert.Equal(snapshot.BaseAttributes, character.BaseAttributes);
        Assert.Equal(snapshot.Gold, character.Gold);
        Assert.Equal(snapshot.Rank, character.FindAbility("power-strike")!.Rank);
        Assert.Same(sword, character.Loadout.Get(Domain.Items.EquipmentSlot.Weapon));
    }

    [Fact]
    public void After_dying_the_character_is_recovered_and_can_start_a_new_expedition()
    {
        var character = Make.Warrior();
        var failed = Start(character);
        failed.EnterRoom(0, Plain);
        character.TakeDamage(character.Health.Current - 1);
        failed.TakeTurn(PlayerAction.BasicAttack(), Plain);

        var next = Start(character);

        Assert.False(character.IsDefeated);
        Assert.True(character.Health.IsFull);
        Assert.Equal(ExpeditionStatus.InProgress, next.Status);
        Assert.Equal(1, next.CurrentFloor.Number);
    }

    [Fact]
    public void Nothing_can_be_done_in_an_expedition_that_is_over()
    {
        var character = Make.Warrior();
        var expedition = Start(character);
        expedition.EnterRoom(0, Plain);
        character.TakeDamage(character.Health.Current - 1);
        expedition.TakeTurn(PlayerAction.BasicAttack(), Plain);

        Assert.Throws<DomainException>(() => expedition.EnterRoom(0, Plain));
        Assert.Throws<DomainException>(() => expedition.TakeTurn(PlayerAction.BasicAttack(), Plain));
    }

    [Fact]
    public void Enemies_are_scaled_to_the_level_the_character_has_when_entering_the_room()
    {
        var novice = Start(Make.Warrior());
        var veteran = Start(Veteran());

        var weak = novice.EnterRoom(0, Plain).StartedCombat!.Enemy;
        var strong = veteran.EnterRoom(0, Plain).StartedCombat!.Enemy;

        Assert.Same(weak.Definition, strong.Definition);
        Assert.Equal(
            EnemyScaling.Scale(weak.Definition.BaseHealth, EnemyScaling.Multiplier(Level.Max, 1)),
            strong.Health.Maximum);
        Assert.True(strong.Attack > weak.Attack);
    }
}
