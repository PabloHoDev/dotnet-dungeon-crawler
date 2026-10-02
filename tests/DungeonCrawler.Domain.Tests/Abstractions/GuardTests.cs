using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Tests.Abstractions;

public class GuardTests
{
    [Fact]
    public void AgainstNull_returns_the_value_when_present()
    {
        var value = new object();

        Assert.Same(value, Guard.AgainstNull(value, "value"));
    }

    [Fact]
    public void AgainstNull_rejects_null_naming_the_parameter()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => Guard.AgainstNull<object>(null, "value"));

        Assert.Equal("value", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AgainstNullOrWhiteSpace_rejects_blank_text(string? text)
    {
        Assert.Throws<ArgumentException>(() => Guard.AgainstNullOrWhiteSpace(text, "text"));
    }

    [Fact]
    public void AgainstNullOrWhiteSpace_returns_the_text_when_present()
    {
        Assert.Equal("Aria", Guard.AgainstNullOrWhiteSpace("Aria", "text"));
    }

    [Fact]
    public void AgainstNegative_accepts_zero_and_rejects_negative_values()
    {
        Assert.Equal(0, Guard.AgainstNegative(0, "amount"));
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.AgainstNegative(-1, "amount"));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(20)]
    public void AgainstOutOfRange_accepts_values_inside_the_inclusive_range(int value)
    {
        Assert.Equal(value, Guard.AgainstOutOfRange(value, 1, 20, "value"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(21)]
    public void AgainstOutOfRange_rejects_values_outside_the_range(int value)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.AgainstOutOfRange(value, 1, 20, "value"));
    }
}
