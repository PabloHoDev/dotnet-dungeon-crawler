using DungeonCrawler.Domain.SharedKernel;
using DungeonCrawler.Domain.Tests.TestDoubles;

namespace DungeonCrawler.Domain.Tests.SharedKernel;

public class ProbabilityTests
{
    [Theory]
    [InlineData(0.0)]
    [InlineData(0.25)]
    [InlineData(1.0)]
    public void A_fraction_between_0_and_1_is_valid(double value)
    {
        Assert.Equal(value, Probability.Of(value).Value);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(1.01)]
    [InlineData(double.NaN)]
    public void A_fraction_outside_0_to_1_is_rejected(double value)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Probability.Of(value));
    }

    [Fact]
    public void A_percentage_is_converted_to_a_fraction()
    {
        Assert.Equal(Probability.Of(0.6), Probability.FromPercent(60));
        Assert.Equal(Probability.Never, Probability.FromPercent(0));
        Assert.Equal(Probability.Always, Probability.FromPercent(100));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(100.5)]
    [InlineData(double.NaN)]
    public void A_percentage_outside_0_to_100_is_rejected(double percent)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Probability.FromPercent(percent));
    }

    [Theory]
    [InlineData(-0.5, 0.0)]
    [InlineData(0.4, 0.4)]
    [InlineData(1.7, 1.0)]
    [InlineData(double.PositiveInfinity, 1.0)]
    public void Clamp_limits_formula_results_to_the_valid_range(double value, double expected)
    {
        Assert.Equal(expected, Probability.Clamp(value).Value);
    }

    [Fact]
    public void Clamp_rejects_NaN()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Probability.Clamp(double.NaN));
    }

    [Fact]
    public void CappedAt_limits_a_chance_to_a_maximum()
    {
        var cap = Probability.Of(0.5);

        Assert.Equal(cap, Probability.Of(0.8).CappedAt(cap));
        Assert.Equal(Probability.Of(0.3), Probability.Of(0.3).CappedAt(cap));
    }

    [Theory]
    [InlineData(0.0, true)]
    [InlineData(0.29, true)]
    [InlineData(0.3, false)]
    [InlineData(0.99, false)]
    public void A_roll_succeeds_only_when_it_falls_below_the_chance(double roll, bool expected)
    {
        var chance = Probability.Of(0.3);

        Assert.Equal(expected, chance.Roll(new FixedRandomSource(roll)));
    }

    [Fact]
    public void Never_fails_even_on_the_lowest_roll_and_Always_succeeds_even_on_the_highest()
    {
        Assert.False(Probability.Never.Roll(new FixedRandomSource(0.0)));
        Assert.True(Probability.Always.Roll(new FixedRandomSource(0.999999)));
    }

    [Fact]
    public void Probabilities_are_ordered_by_value()
    {
        Assert.True(Probability.Of(0.1).CompareTo(Probability.Of(0.2)) < 0);
        Assert.Equal(0, Probability.Of(0.2).CompareTo(Probability.Of(0.2)));
    }

    [Fact]
    public void Text_form_is_a_culture_independent_percentage()
    {
        Assert.Equal("12.5%", Probability.Of(0.125).ToString());
    }
}
