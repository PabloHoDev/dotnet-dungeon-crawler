using DungeonCrawler.Domain.Enemies;
using DungeonCrawler.Domain.Enemies.Behaviors;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.SharedKernel;
using DungeonCrawler.Domain.Tests.TestDoubles;

namespace DungeonCrawler.Domain.Tests.Enemies;

public class EnemyFactoryTests
{
    [Fact]
    public void At_level_1_on_floor_1_an_enemy_has_its_base_statistics()
    {
        var goblin = EnemyFactory.Create(Bestiary.Goblin, null, Level.First, 1);

        Assert.Equal("Goblin", goblin.Name);
        Assert.Equal(EnemyCategory.Common, goblin.Category);
        Assert.Equal(Bestiary.Goblin.BaseHealth, goblin.Health.Maximum);
        Assert.Equal(Bestiary.Goblin.BaseAttack, goblin.Attack);
        Assert.Equal(Bestiary.Goblin.BaseDefense, goblin.Defense);
        Assert.Equal(Bestiary.Goblin.BaseExperience, goblin.ExperienceReward);
        Assert.Equal(Bestiary.Goblin.BaseGold, goblin.GoldReward);
        Assert.True(goblin.Health.IsFull);
    }

    [Fact]
    public void Health_attack_and_rewards_scale_with_player_level_and_floor()
    {
        var multiplier = EnemyScaling.Multiplier(Level.Of(6), 4);

        var orc = EnemyFactory.Create(Bestiary.Orc, null, Level.Of(6), 4);

        Assert.Equal(EnemyScaling.Scale(Bestiary.Orc.BaseHealth, multiplier), orc.Health.Maximum);
        Assert.Equal(EnemyScaling.Scale(Bestiary.Orc.BaseAttack, multiplier), orc.Attack);
        Assert.Equal(EnemyScaling.Scale(Bestiary.Orc.BaseExperience, multiplier), orc.ExperienceReward);
        Assert.Equal(EnemyScaling.Scale(Bestiary.Orc.BaseGold, multiplier), orc.GoldReward);
    }

    [Fact]
    public void Defense_scales_with_the_floor_only()
    {
        var lowLevel = EnemyFactory.Create(Bestiary.Orc, null, Level.Of(1), 1);
        var highLevel = EnemyFactory.Create(Bestiary.Orc, null, Level.Of(15), 1);
        var deeper = EnemyFactory.Create(Bestiary.Orc, null, Level.Of(1), 5);

        Assert.Equal(lowLevel.Defense, highLevel.Defense);
        Assert.True(deeper.Defense > lowLevel.Defense);
    }

    [Fact]
    public void The_same_enemy_is_stronger_on_a_deeper_floor_and_against_a_stronger_player()
    {
        var baseline = EnemyFactory.Create(Bestiary.Skeleton, null, Level.Of(3), 2);
        var deeper = EnemyFactory.Create(Bestiary.Skeleton, null, Level.Of(3), 3);
        var higherLevel = EnemyFactory.Create(Bestiary.Skeleton, null, Level.Of(4), 2);

        Assert.True(deeper.Health.Maximum > baseline.Health.Maximum && deeper.Attack > baseline.Attack);
        Assert.True(higherLevel.Health.Maximum > baseline.Health.Maximum && higherLevel.Attack > baseline.Attack);
    }

    [Fact]
    public void The_elite_modifier_turns_a_common_enemy_into_a_stronger_more_rewarding_one()
    {
        var goblin = EnemyFactory.Create(Bestiary.Goblin, null, Level.First, 1);

        var elite = EnemyFactory.Create(Bestiary.Goblin, EnemyModifier.Elite, Level.First, 1);

        Assert.Equal("Elite Goblin", elite.Name);
        Assert.Equal(EnemyCategory.Elite, elite.Category);
        Assert.Same(Bestiary.Goblin, elite.Definition);
        Assert.True(elite.Health.Maximum > goblin.Health.Maximum);
        Assert.True(elite.Attack > goblin.Attack);
        Assert.True(elite.Defense > goblin.Defense);
        Assert.True(elite.ExperienceReward > goblin.ExperienceReward);
        Assert.True(elite.GoldReward > goblin.GoldReward);
    }

    [Fact]
    public void The_bestiary_covers_every_category_with_stronger_enemies_at_the_top()
    {
        Assert.Equal(EnemyCategory.Common, Bestiary.Goblin.Category);
        Assert.Equal(EnemyCategory.Common, Bestiary.Skeleton.Category);
        Assert.Equal(EnemyCategory.Common, Bestiary.Orc.Category);
        Assert.Equal(EnemyCategory.MiniBoss, Bestiary.SkeletonKnight.Category);
        Assert.Equal(EnemyCategory.Boss, Bestiary.CryptLord.Category);

        Assert.True(Bestiary.SkeletonKnight.BaseHealth > Bestiary.Orc.BaseHealth);
        Assert.True(Bestiary.CryptLord.BaseHealth > Bestiary.SkeletonKnight.BaseHealth);
        Assert.True(Bestiary.CryptLord.BaseExperience > Bestiary.SkeletonKnight.BaseExperience);
    }
}

public class EnemyBehaviorTests
{
    private static readonly ConstantRandomSource Random = ConstantRandomSource.NothingHappens;

    /// <summary>Lets the enemy decide and registers the turn, as a combat round would.</summary>
    private static EnemyIntent TakeTurn(Enemy enemy)
    {
        var intent = enemy.DecideIntent(Make.Warrior(), Random);
        enemy.CompleteTurn(intent);
        return intent;
    }

    [Fact]
    public void An_aggressive_enemy_opens_with_its_ability_and_reuses_it_when_the_cooldown_ends()
    {
        var goblin = Make.Enemy(Bestiary.Goblin);

        var actions = Enumerable.Range(0, 9).Select(_ => TakeTurn(goblin).ActionName).ToArray();

        // Stab has a cooldown of 3: three basic attacks between each use.
        Assert.Equal(
            new[] { "Stab", "Attack", "Attack", "Attack", "Stab", "Attack", "Attack", "Attack", "Stab" },
            actions);
    }

    [Fact]
    public void An_ability_hits_harder_than_a_basic_attack()
    {
        var orc = Make.Enemy(Bestiary.Orc);

        var special = TakeTurn(orc);
        var basic = TakeTurn(orc);

        Assert.NotNull(special.Ability);
        Assert.Null(basic.Ability);
        Assert.True(orc.AttackDamage(special.PowerMultiplier) > orc.AttackDamage(basic.PowerMultiplier));
    }

    [Fact]
    public void A_healthy_skirmisher_only_attacks()
    {
        var skeleton = Make.Enemy(Bestiary.Skeleton);

        var kinds = Enumerable.Range(0, 6).Select(_ => TakeTurn(skeleton).Kind);

        Assert.All(kinds, kind => Assert.Equal(EnemyIntentKind.Attack, kind));
    }

    [Fact]
    public void A_wounded_skirmisher_falls_back_every_other_turn()
    {
        var skeleton = Make.Enemy(Bestiary.Skeleton);
        skeleton.TakeDamage((skeleton.Health.Maximum / 2) + 1);

        var kinds = Enumerable.Range(0, 4).Select(_ => TakeTurn(skeleton).Kind).ToArray();

        Assert.Equal(
            new[] { EnemyIntentKind.Attack, EnemyIntentKind.Guard, EnemyIntentKind.Attack, EnemyIntentKind.Guard },
            kinds);
    }

    [Fact]
    public void A_tank_raises_its_guard_every_third_turn()
    {
        var knight = Make.Enemy(Bestiary.SkeletonKnight);

        var kinds = Enumerable.Range(0, 6).Select(_ => TakeTurn(knight).Kind).ToArray();

        Assert.Equal(
            new[]
            {
                EnemyIntentKind.Attack, EnemyIntentKind.Attack, EnemyIntentKind.Guard,
                EnemyIntentKind.Attack, EnemyIntentKind.Attack, EnemyIntentKind.Guard,
            },
            kinds);
    }

    [Fact]
    public void The_boss_fights_with_its_first_ability_while_above_half_health()
    {
        var boss = Make.Enemy(Bestiary.CryptLord);

        var opening = TakeTurn(boss);
        var followUp = TakeTurn(boss);

        Assert.Equal("Dark Bolt", opening.ActionName);
        Assert.Equal(1.6, opening.PowerMultiplier, precision: 10);
        Assert.Equal("Attack", followUp.ActionName);
        Assert.Equal(1.0, followUp.PowerMultiplier, precision: 10);
    }

    [Fact]
    public void Below_half_health_the_boss_enrages_and_switches_to_its_second_ability()
    {
        var boss = Make.Enemy(Bestiary.CryptLord);
        boss.TakeDamage(boss.Health.Maximum / 2);

        var special = TakeTurn(boss);
        var basic = TakeTurn(boss);

        Assert.True(((BossBehavior)boss.Definition.Behavior).IsEnraged(boss));
        Assert.Equal("Soul Drain", special.ActionName);
        Assert.Equal(1.4 * 1.25, special.PowerMultiplier, precision: 10);
        Assert.True(special.Ability!.LifeStealFraction > 0);
        Assert.Equal(1.25, basic.PowerMultiplier, precision: 10);
    }

    [Fact]
    public void Different_enemies_share_the_same_behavior_type()
    {
        Assert.IsType<AggressiveBehavior>(Bestiary.Goblin.Behavior);
        Assert.IsType<AggressiveBehavior>(Bestiary.Orc.Behavior);
        Assert.IsType<SkirmisherBehavior>(Bestiary.Skeleton.Behavior);
        Assert.IsType<TankBehavior>(Bestiary.SkeletonKnight.Behavior);
        Assert.IsType<BossBehavior>(Bestiary.CryptLord.Behavior);
    }
}
