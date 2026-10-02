using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Enemies;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.SharedKernel;
using DungeonCrawler.Domain.Tests.TestDoubles;

namespace DungeonCrawler.Domain.Tests.Combat;

public class AttackResolverTests
{
    private const double Miss = 0.99;
    private const double Success = 0.0;

    private static AttackProfile Attack(int damage, double criticalChance = 0.5)
    {
        return new AttackProfile(damage, Probability.Of(criticalChance), 2.0);
    }

    [Fact]
    public void A_normal_hit_deals_raw_damage_reduced_by_the_targets_defense()
    {
        var goblin = Make.Enemy(Bestiary.Goblin);
        var expected = DamageFormula.Mitigate(40, goblin.Defense);

        var outcome = AttackResolver.Resolve(Attack(40), goblin, new FixedRandomSource(Miss, Miss));

        Assert.False(outcome.WasCritical);
        Assert.False(outcome.WasEvaded);
        Assert.Equal(expected, outcome.Damage);
        Assert.Equal(goblin.Health.Maximum - expected, goblin.Health.Current);
    }

    [Fact]
    public void A_critical_hit_multiplies_the_damage_before_defense()
    {
        var goblin = Make.Enemy(Bestiary.Goblin);
        var expected = DamageFormula.Mitigate(DamageFormula.Critical(20, 2.0), goblin.Defense);

        var outcome = AttackResolver.Resolve(Attack(20), goblin, new FixedRandomSource(Miss, Success));

        Assert.True(outcome.WasCritical);
        Assert.Equal(expected, outcome.Damage);
    }

    [Fact]
    public void An_attack_without_critical_chance_never_crits()
    {
        var goblin = Make.Enemy(Bestiary.Goblin);

        var outcome = AttackResolver.Resolve(Attack(20, criticalChance: 0.0), goblin, new FixedRandomSource(Miss, Success));

        Assert.False(outcome.WasCritical);
    }

    [Fact]
    public void An_evaded_attack_deals_no_damage()
    {
        var rogue = Make.Rogue();

        var outcome = AttackResolver.Resolve(Attack(40), rogue, new FixedRandomSource(Success));

        Assert.True(outcome.WasEvaded);
        Assert.Equal(0, outcome.Damage);
        Assert.True(rogue.Health.IsFull);
    }

    [Fact]
    public void A_target_without_evasion_is_always_hit()
    {
        var goblin = Make.Enemy(Bestiary.Goblin);

        var outcome = AttackResolver.Resolve(Attack(40), goblin, new FixedRandomSource(Success, Miss));

        Assert.False(outcome.WasEvaded);
    }

    [Fact]
    public void A_guarding_target_takes_half_the_damage()
    {
        var open = Make.Warrior();
        var guarded = Make.Warrior();
        guarded.StartGuarding();

        var normal = AttackResolver.Resolve(Attack(60), open, new FixedRandomSource(Miss, Miss));
        var reduced = AttackResolver.Resolve(Attack(60), guarded, new FixedRandomSource(Miss, Miss));

        Assert.True(reduced.WasGuarded);
        Assert.Equal(DamageFormula.Guarded(normal.Damage), reduced.Damage);
    }

    [Fact]
    public void More_defense_means_less_damage_taken()
    {
        var bare = Make.Warrior();
        var armored = Make.Warrior();
        var armor = Make.Armor(defense: 30, maxHealth: 0);
        armored.Store(armor);
        armored.Equip(armor.Id);

        var againstBare = AttackResolver.Resolve(Attack(60), bare, new FixedRandomSource(Miss, Miss));
        var againstArmored = AttackResolver.Resolve(Attack(60), armored, new FixedRandomSource(Miss, Miss));

        Assert.True(againstArmored.Damage < againstBare.Damage);
    }

    [Fact]
    public void A_critical_multiplier_below_1_is_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new AttackProfile(10, Probability.Never, 0.5));
    }

    [Fact]
    public void A_hit_can_defeat_the_target()
    {
        var character = Character.Create(Guid.NewGuid(), "Aria", CharacterClass.Mage);

        AttackResolver.Resolve(Attack(100000), character, new FixedRandomSource(Miss, Miss));

        Assert.True(character.IsDefeated);
    }
}
