using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Tests.Progression;

public class ExperienceCurveTests
{
    [Theory]
    [InlineData(1, 100)]
    [InlineData(2, 300)]
    [InlineData(3, 600)]
    [InlineData(4, 1000)]
    [InlineData(19, 19000)]
    public void Completing_level_n_takes_50_n_n_plus_1_accumulated_experience(int level, int expected)
    {
        Assert.Equal(expected, ExperienceCurve.TotalToComplete(Level.Of(level)));
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 100)]
    [InlineData(3, 300)]
    [InlineData(20, 19000)]
    public void A_level_starts_where_the_previous_one_is_completed(int level, int expected)
    {
        Assert.Equal(expected, ExperienceCurve.TotalToReach(Level.Of(level)));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(7)]
    [InlineData(19)]
    public void Each_level_costs_100_times_its_number(int level)
    {
        Assert.Equal(100 * level, ExperienceCurve.CostOf(Level.Of(level)));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(99, 1)]
    [InlineData(100, 2)]
    [InlineData(299, 2)]
    [InlineData(300, 3)]
    [InlineData(18999, 19)]
    [InlineData(19000, 20)]
    [InlineData(1000000, 20)]
    public void The_level_follows_from_the_accumulated_experience(int experience, int expectedLevel)
    {
        Assert.Equal(Level.Of(expectedLevel), ExperienceCurve.LevelFor(experience));
    }

    [Fact]
    public void The_maximum_level_is_reached_with_19000_experience()
    {
        Assert.Equal(19000, ExperienceCurve.MaxTotal);
    }

    [Fact]
    public void Remaining_experience_counts_down_to_the_next_level()
    {
        Assert.Equal(100, ExperienceCurve.RemainingToNextLevel(0));
        Assert.Equal(1, ExperienceCurve.RemainingToNextLevel(99));
        Assert.Equal(200, ExperienceCurve.RemainingToNextLevel(100));
        Assert.Equal(0, ExperienceCurve.RemainingToNextLevel(19000));
    }

    [Fact]
    public void Progress_is_the_fraction_of_the_current_level_already_earned()
    {
        Assert.Equal(0.0, ExperienceCurve.ProgressInLevel(0));
        Assert.Equal(0.5, ExperienceCurve.ProgressInLevel(50));
        Assert.Equal(0.25, ExperienceCurve.ProgressInLevel(150));
        Assert.Equal(1.0, ExperienceCurve.ProgressInLevel(19000));
    }

    [Fact]
    public void Negative_experience_is_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ExperienceCurve.LevelFor(-1));
    }
}
