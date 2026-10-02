using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Tests.SharedKernel;

public class LevelTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(20)]
    public void A_level_between_1_and_20_is_valid(int value)
    {
        Assert.Equal(value, Level.Of(value).Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(21)]
    public void A_level_outside_1_to_20_is_rejected(int value)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Level.Of(value));
    }

    [Fact]
    public void The_first_level_is_1_and_the_maximum_is_20()
    {
        Assert.Equal(1, Level.First.Value);
        Assert.Equal(20, Level.Max.Value);
        Assert.False(Level.First.IsMax);
        Assert.True(Level.Max.IsMax);
    }

    [Fact]
    public void Next_returns_the_following_level()
    {
        Assert.Equal(Level.Of(6), Level.Of(5).Next());
        Assert.Equal(Level.Max, Level.Of(19).Next());
    }

    [Fact]
    public void The_maximum_level_has_no_next_level()
    {
        Assert.Throws<DomainException>(() => Level.Max.Next());
    }

    [Fact]
    public void Levels_with_the_same_value_are_equal()
    {
        Assert.Equal(Level.Of(7), Level.Of(7));
        Assert.NotEqual(Level.Of(7), Level.Of(8));
    }

    [Fact]
    public void Levels_are_ordered_by_value()
    {
        Assert.True(Level.Of(3).CompareTo(Level.Of(4)) < 0);
        Assert.True(Level.Of(4).CompareTo(Level.Of(3)) > 0);
        Assert.Equal(0, Level.Of(4).CompareTo(Level.Of(4)));
    }
}
