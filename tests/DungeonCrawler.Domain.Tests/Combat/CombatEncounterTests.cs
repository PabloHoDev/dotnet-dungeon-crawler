using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Enemies;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.SharedKernel;
using DungeonCrawler.Domain.Tests.TestDoubles;

namespace DungeonCrawler.Domain.Tests.Combat;

public class CombatEncounterTests
{
    private static readonly IRandomSource Plain = ConstantRandomSource.NothingHappens;

    [Fact]
    public void A_combat_starts_in_progress_at_round_zero()
    {
        var combat = new CombatEncounter(Make.Warrior(), Make.Enemy(Bestiary.Goblin));

        Assert.Equal(CombatStatus.InProgress, combat.Status);
        Assert.Equal(0, combat.Round);
        Assert.False(combat.IsOver);
    }

    [Fact]
    public void A_defeated_character_cannot_start_a_combat()
    {
        var character = Make.Warrior();
        character.TakeDamage(100000);

        Assert.Throws<DomainException>(() => new CombatEncounter(character, Make.Enemy(Bestiary.Goblin)));
    }

    [Fact]
    public void In_a_round_both_sides_act_once()
    {
        var player = Make.Warrior();
        var goblin = Make.Enemy(Bestiary.Goblin);
        var combat = new CombatEncounter(player, goblin);

        var round = combat.ExecuteRound(PlayerAction.BasicAttack(), Plain);

        Assert.Equal(1, round.Round);
        Assert.Equal(2, round.Log.Count);
        Assert.Single(round.Log, entry => entry.ActorIsPlayer);
        Assert.Single(round.Log, entry => !entry.ActorIsPlayer);
        Assert.True(goblin.Health.Current < goblin.Health.Maximum);
        Assert.True(player.Health.Current < player.Health.Maximum);
    }

    [Fact]
    public void A_basic_attack_deals_the_characters_basic_damage_reduced_by_defense()
    {
        var player = Make.Warrior();
        var goblin = Make.Enemy(Bestiary.Goblin);
        var expected = DamageFormula.Mitigate(player.BasicAttackDamage, goblin.Defense);

        new CombatEncounter(player, goblin).ExecuteRound(PlayerAction.BasicAttack(), Plain);

        Assert.Equal(goblin.Health.Maximum - expected, goblin.Health.Current);
    }

    [Fact]
    public void The_faster_side_acts_first_and_ties_favor_the_player()
    {
        var slowPlayerCombat = new CombatEncounter(Make.Warrior(), Make.Enemy(Bestiary.Goblin));
        var fastPlayerCombat = new CombatEncounter(Make.Rogue(), Make.Enemy(Bestiary.Goblin));
        var tiedCombat = new CombatEncounter(Make.Mage(), Make.Enemy(Bestiary.Skeleton));

        Assert.False(slowPlayerCombat.PlayerActsFirst);
        Assert.True(fastPlayerCombat.PlayerActsFirst);
        Assert.True(tiedCombat.PlayerActsFirst);

        Assert.False(slowPlayerCombat.ExecuteRound(PlayerAction.BasicAttack(), Plain).Log[0].ActorIsPlayer);
        Assert.True(fastPlayerCombat.ExecuteRound(PlayerAction.BasicAttack(), Plain).Log[0].ActorIsPlayer);
    }

    [Fact]
    public void An_ability_spends_resource_and_hits_harder_than_a_basic_attack()
    {
        var player = Make.Warrior();
        var goblin = Make.Enemy(Bestiary.Goblin);
        var ability = player.Abilities[0];
        var resourceBefore = player.Resource.Current;

        var round = new CombatEncounter(player, goblin).ExecuteRound(PlayerAction.UseAbility(ability.Definition.Id), Plain);

        var playerEntry = Assert.Single(round.Log, entry => entry.ActorIsPlayer);
        Assert.Equal(ability.Definition.Name, playerEntry.ActionName);
        Assert.True(playerEntry.Amount > DamageFormula.Mitigate(player.BasicAttackDamage, goblin.Defense));

        // Cost paid, then the end-of-round regeneration.
        Assert.Equal(
            resourceBefore - ability.Definition.Cost + player.Class.ResourceRegeneration,
            player.Resource.Current);
    }

    [Fact]
    public void An_ability_that_was_not_learned_cannot_be_used()
    {
        var combat = new CombatEncounter(Make.Warrior(), Make.Enemy(Bestiary.Goblin));

        Assert.Throws<DomainException>(() => combat.ExecuteRound(PlayerAction.UseAbility("crushing-blow"), Plain));
        Assert.Equal(0, combat.Round);
    }

    [Fact]
    public void An_ability_cannot_be_used_without_enough_resource()
    {
        var player = Make.Warrior();
        player.SpendResource(player.Resource.Current);
        var combat = new CombatEncounter(player, Make.Enemy(Bestiary.Goblin));

        Assert.Throws<DomainException>(() => combat.ExecuteRound(PlayerAction.UseAbility("power-strike"), Plain));
        Assert.True(player.Health.IsFull);
    }

    [Fact]
    public void A_healing_ability_restores_the_characters_health()
    {
        var player = Make.Warrior();
        player.GainExperience(ExperienceCurve.TotalToReach(Level.Of(3)));
        player.ImproveAbility("second-wind");
        player.TakeDamage(100);
        var combat = new CombatEncounter(player, Make.Enemy(Bestiary.Orc));

        var round = combat.ExecuteRound(PlayerAction.UseAbility("second-wind"), Plain);

        var heal = Assert.Single(round.Log, entry => entry.Kind == CombatLogKind.Heal);
        Assert.True(heal.Amount > 0);
        Assert.True(heal.ActorIsPlayer);
    }

    [Fact]
    public void Defending_halves_the_damage_taken_and_recovers_extra_resource()
    {
        var attacker = Make.Warrior();
        var defender = Make.Warrior();
        defender.SpendResource(20);

        new CombatEncounter(attacker, Make.Enemy(Bestiary.Goblin)).ExecuteRound(PlayerAction.BasicAttack(), Plain);
        var round = new CombatEncounter(defender, Make.Enemy(Bestiary.Goblin)).ExecuteRound(PlayerAction.Defend(), Plain);

        var damageTakenAttacking = attacker.Health.Maximum - attacker.Health.Current;
        var damageTakenDefending = defender.Health.Maximum - defender.Health.Current;

        Assert.Equal(DamageFormula.Guarded(damageTakenAttacking), damageTakenDefending);
        Assert.Contains(round.Log, entry => entry.Kind == CombatLogKind.Guard && entry.ActorIsPlayer);
        Assert.Equal(defender.MaxResource - 20 + (2 * defender.Class.ResourceRegeneration), defender.Resource.Current);
        Assert.False(defender.IsGuarding);
    }

    [Fact]
    public void Defeating_the_enemy_ends_the_combat_in_victory()
    {
        var player = Make.Warrior();
        var goblin = Make.Enemy(Bestiary.Goblin);
        var combat = new CombatEncounter(player, goblin);

        RoundResult round;
        do
        {
            round = combat.ExecuteRound(PlayerAction.BasicAttack(), Plain);
        }
        while (round.Status == CombatStatus.InProgress);

        Assert.Equal(CombatStatus.Victory, combat.Status);
        Assert.True(goblin.IsDefeated);
        Assert.False(player.IsDefeated);
    }

    [Fact]
    public void An_enemy_defeated_before_its_turn_does_not_act()
    {
        var player = Make.Rogue();
        var goblin = Make.Enemy(Bestiary.Goblin);
        goblin.TakeDamage(goblin.Health.Current - 1);

        var round = new CombatEncounter(player, goblin).ExecuteRound(PlayerAction.BasicAttack(), Plain);

        Assert.Equal(CombatStatus.Victory, round.Status);
        Assert.Single(round.Log);
        Assert.True(player.Health.IsFull);
    }

    [Fact]
    public void Running_out_of_health_ends_the_combat_in_defeat()
    {
        var player = Make.Warrior();
        player.TakeDamage(player.Health.Current - 1);
        var combat = new CombatEncounter(player, Make.Enemy(Bestiary.Goblin));

        var round = combat.ExecuteRound(PlayerAction.BasicAttack(), Plain);

        Assert.Equal(CombatStatus.Defeat, round.Status);
        Assert.True(player.IsDefeated);
        Assert.Single(round.Log);
    }

    [Fact]
    public void No_more_rounds_can_be_played_after_the_combat_ends()
    {
        var player = Make.Warrior();
        player.TakeDamage(player.Health.Current - 1);
        var combat = new CombatEncounter(player, Make.Enemy(Bestiary.Goblin));
        combat.ExecuteRound(PlayerAction.BasicAttack(), Plain);

        Assert.Throws<DomainException>(() => combat.ExecuteRound(PlayerAction.BasicAttack(), Plain));
    }

    [Fact]
    public void The_same_seed_replays_the_same_combat()
    {
        static List<(string Action, int Amount, bool Critical)> Play(int seed)
        {
            var random = new SeededRandomSource(seed);
            var combat = new CombatEncounter(Make.Rogue(), Make.Enemy(Bestiary.Orc));
            var log = new List<(string, int, bool)>();

            while (!combat.IsOver)
            {
                foreach (var entry in combat.ExecuteRound(PlayerAction.BasicAttack(), random).Log)
                {
                    log.Add((entry.ActionName, entry.Amount, entry.WasCritical));
                }
            }

            return log;
        }

        Assert.Equal(Play(99), Play(99));
    }
}
