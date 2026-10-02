using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Tests.Progression;

public class StatFormulasTests
{
    [Fact]
    public void Max_health_is_base_plus_vitality_scaling_plus_level_growth_plus_equipment()
    {
        // 60 + 8 × 8 + 3 × 6 + 20
        Assert.Equal(162, StatFormulas.MaxHealth(baseHealth: 60, vitality: 8, Level.Of(3), equipmentBonus: 20));
    }

    [Fact]
    public void Defense_is_vitality_plus_equipment()
    {
        Assert.Equal(15, StatFormulas.Defense(vitality: 10, equipmentBonus: 5));
    }

    [Fact]
    public void Max_resource_grows_with_intelligence_only_for_classes_that_scale_with_it()
    {
        Assert.Equal(40, StatFormulas.MaxResource(baseResource: 20, intelligence: 10, resourcePerIntelligence: 2));
        Assert.Equal(30, StatFormulas.MaxResource(baseResource: 30, intelligence: 10, resourcePerIntelligence: 0));
    }

    [Fact]
    public void Critical_chance_grows_one_percent_per_point_of_luck()
    {
        Assert.Equal(0.15, StatFormulas.CriticalChance(Probability.Of(0.05), luck: 10).Value, precision: 10);
    }

    [Fact]
    public void Critical_chance_never_exceeds_the_cap()
    {
        Assert.Equal(Balance.MaxCriticalChance, StatFormulas.CriticalChance(Probability.Of(0.10), luck: 500).Value);
    }

    [Fact]
    public void Evasion_grows_one_percent_per_point_of_agility_up_to_the_cap()
    {
        Assert.Equal(0.09, StatFormulas.Evasion(agility: 9).Value, precision: 10);
        Assert.Equal(Balance.MaxEvasion, StatFormulas.Evasion(agility: 500).Value);
    }

    [Fact]
    public void Basic_attack_scales_with_the_class_attribute_and_the_weapon()
    {
        // 4 + 8 × 1.5 + 6
        Assert.Equal(22, StatFormulas.BasicAttackDamage(attribute: 8, equipmentBonus: 6));
    }
}

public class DamageFormulaTests
{
    [Fact]
    public void Raw_damage_adds_base_attribute_scaling_rank_growth_and_equipment()
    {
        // 8 + 10 × 2.0 + 2 × 6 + 5
        Assert.Equal(45, DamageFormula.Raw(baseDamage: 8, attribute: 10, scaling: 2.0, rank: 2, growth: 6, equipmentBonus: 5));
    }

    [Fact]
    public void A_higher_rank_always_deals_more_damage()
    {
        var rank1 = DamageFormula.Raw(8, 10, 2.0, 1, 6, 0);
        var rank2 = DamageFormula.Raw(8, 10, 2.0, 2, 6, 0);
        var rank3 = DamageFormula.Raw(8, 10, 2.0, 3, 6, 0);

        Assert.True(rank1 < rank2 && rank2 < rank3);
    }

    [Fact]
    public void A_critical_hit_multiplies_the_damage()
    {
        Assert.Equal(60, DamageFormula.Critical(40, 1.5));
    }

    [Theory]
    [InlineData(100, 0, 100)]
    [InlineData(100, 40, 50)]
    [InlineData(100, 120, 25)]
    public void Defense_reduces_damage_with_diminishing_returns(int damage, int defense, int expected)
    {
        Assert.Equal(expected, DamageFormula.Mitigate(damage, defense));
    }

    [Fact]
    public void Defense_never_reduces_damage_below_the_minimum()
    {
        Assert.Equal(Balance.MinimumDamage, DamageFormula.Mitigate(1, 100000));
        Assert.Equal(Balance.MinimumDamage, DamageFormula.Mitigate(0, 0));
    }

    [Fact]
    public void Guarding_halves_the_damage()
    {
        Assert.Equal(20, DamageFormula.Guarded(40));
        Assert.Equal(Balance.MinimumDamage, DamageFormula.Guarded(1));
    }
}

public class EnemyScalingTests
{
    [Fact]
    public void A_level_1_player_on_floor_1_faces_enemies_at_base_power()
    {
        Assert.Equal(1.0, EnemyScaling.Multiplier(Level.First, 1));
    }

    [Fact]
    public void Enemy_power_is_base_times_level_multiplier_times_floor_multiplier()
    {
        var expected = EnemyScaling.LevelMultiplier(Level.Of(6)) * EnemyScaling.FloorMultiplier(4);

        Assert.Equal(expected, EnemyScaling.Multiplier(Level.Of(6), 4));
        Assert.Equal(1.0 + (5 * Balance.EnemyLevelScalingStep), EnemyScaling.LevelMultiplier(Level.Of(6)), precision: 10);
        Assert.Equal(1.0 + (3 * Balance.EnemyFloorScalingStep), EnemyScaling.FloorMultiplier(4), precision: 10);
    }

    [Fact]
    public void Enemies_get_stronger_with_every_level_and_every_floor()
    {
        for (var level = 1; level < Level.MaxValue; level++)
        {
            Assert.True(EnemyScaling.LevelMultiplier(Level.Of(level + 1)) > EnemyScaling.LevelMultiplier(Level.Of(level)));
        }

        for (var floor = 1; floor < 5; floor++)
        {
            Assert.True(EnemyScaling.FloorMultiplier(floor + 1) > EnemyScaling.FloorMultiplier(floor));
        }
    }

    [Fact]
    public void Floors_start_at_1()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => EnemyScaling.FloorMultiplier(0));
    }

    [Fact]
    public void The_same_input_always_gives_the_same_output()
    {
        Assert.Equal(EnemyScaling.Scale(45, 1.37), EnemyScaling.Scale(45, 1.37));
        Assert.Equal(62, EnemyScaling.Scale(45, 1.37));
    }
}
