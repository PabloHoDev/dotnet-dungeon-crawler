using DungeonCrawler.Domain.Items;
using DungeonCrawler.Domain.Progression;

namespace DungeonCrawler.Domain.Tests.Progression;

public class LootProbabilityTests
{
    [Fact]
    public void Without_luck_the_weights_are_60_25_10_4_1()
    {
        Assert.Equal(new[] { 60.0, 25.0, 10.0, 4.0, 1.0 }, LootProbability.WeightsFor(luck: 0));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(17)]
    [InlineData(40)]
    [InlineData(1000)]
    public void The_weights_always_add_up_to_100(int luck)
    {
        Assert.Equal(100.0, LootProbability.WeightsFor(luck).Sum(), precision: 9);
    }

    [Fact]
    public void Luck_moves_weight_from_Common_to_every_better_rarity()
    {
        var unlucky = LootProbability.WeightsFor(0);
        var lucky = LootProbability.WeightsFor(10);

        Assert.True(lucky[(int)Rarity.Common] < unlucky[(int)Rarity.Common]);
        for (var rarity = (int)Rarity.Uncommon; rarity <= (int)Rarity.Legendary; rarity++)
        {
            Assert.True(lucky[rarity] > unlucky[rarity]);
        }
    }

    [Fact]
    public void The_influence_of_luck_is_limited()
    {
        var atTheLimit = LootProbability.WeightsFor(40);
        var farBeyond = LootProbability.WeightsFor(4000);

        Assert.Equal(atTheLimit, farBeyond);
        Assert.Equal(60.0 - Balance.MaxLuckRarityShift, farBeyond[(int)Rarity.Common], precision: 9);
    }

    [Theory]
    [InlineData(0.0, Rarity.Common)]
    [InlineData(0.5999, Rarity.Common)]
    [InlineData(0.60, Rarity.Uncommon)]
    [InlineData(0.8499, Rarity.Uncommon)]
    [InlineData(0.85, Rarity.Rare)]
    [InlineData(0.95, Rarity.Epic)]
    [InlineData(0.99, Rarity.Legendary)]
    [InlineData(0.9999, Rarity.Legendary)]
    public void A_roll_selects_the_rarity_whose_band_it_falls_in(double roll, Rarity expected)
    {
        Assert.Equal(expected, LootProbability.Pick(roll, luck: 0));
    }

    [Fact]
    public void The_same_roll_can_give_a_better_rarity_to_a_luckier_character()
    {
        Assert.Equal(Rarity.Common, LootProbability.Pick(0.55, luck: 0));
        Assert.Equal(Rarity.Uncommon, LootProbability.Pick(0.55, luck: 20));
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.0)]
    [InlineData(double.NaN)]
    public void A_roll_outside_zero_to_one_is_rejected(double roll)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => LootProbability.Pick(roll, luck: 0));
    }

    [Fact]
    public void Over_many_rolls_the_observed_distribution_matches_the_weights()
    {
        var random = new Random(12345);
        var counts = new int[5];
        const int rolls = 100000;

        for (var i = 0; i < rolls; i++)
        {
            counts[(int)LootProbability.Pick(random.NextDouble(), luck: 0)]++;
        }

        var expected = LootProbability.WeightsFor(0);
        for (var rarity = 0; rarity < counts.Length; rarity++)
        {
            Assert.Equal(expected[rarity], counts[rarity] * 100.0 / rolls, tolerance: 0.5);
        }
    }
}
