using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Tests.SharedKernel;

public class HealthTests
{
    [Fact]
    public void Full_health_starts_at_the_maximum()
    {
        var health = Health.Full(100);

        Assert.Equal(100, health.Current);
        Assert.Equal(100, health.Maximum);
        Assert.True(health.IsFull);
        Assert.False(health.IsDepleted);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Maximum_health_must_be_at_least_1(int maximum)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Health.Full(maximum));
    }

    [Theory]
    [InlineData(-1, 100)]
    [InlineData(101, 100)]
    public void Current_health_must_stay_between_zero_and_the_maximum(int current, int maximum)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Health.Of(current, maximum));
    }

    [Fact]
    public void Damage_reduces_current_health()
    {
        Assert.Equal(Health.Of(70, 100), Health.Full(100).TakeDamage(30));
    }

    [Fact]
    public void Damage_beyond_the_remaining_health_stops_at_zero()
    {
        var health = Health.Of(20, 100).TakeDamage(999);

        Assert.Equal(0, health.Current);
        Assert.True(health.IsDepleted);
    }

    [Fact]
    public void Healing_restores_current_health()
    {
        Assert.Equal(Health.Of(60, 100), Health.Of(40, 100).Heal(20));
    }

    [Fact]
    public void Healing_beyond_the_maximum_stops_at_the_maximum()
    {
        Assert.True(Health.Of(90, 100).Heal(50).IsFull);
        Assert.True(Health.Of(90, 100).Heal(int.MaxValue).IsFull);
    }

    [Fact]
    public void Negative_damage_and_healing_are_rejected()
    {
        var health = Health.Full(100);

        Assert.Throws<ArgumentOutOfRangeException>(() => health.TakeDamage(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => health.Heal(-1));
    }

    [Fact]
    public void Operations_return_a_new_value_and_leave_the_original_untouched()
    {
        var original = Health.Full(100);

        original.TakeDamage(30);

        Assert.Equal(100, original.Current);
    }

    [Fact]
    public void Raising_the_maximum_keeps_the_current_health()
    {
        Assert.Equal(Health.Of(80, 150), Health.Of(80, 100).WithMaximum(150));
    }

    [Fact]
    public void Lowering_the_maximum_below_the_current_health_lowers_the_current_health()
    {
        Assert.Equal(Health.Full(50), Health.Of(80, 100).WithMaximum(50));
    }

    [Fact]
    public void Fraction_reports_the_remaining_share_of_health()
    {
        Assert.Equal(0.25, Health.Of(25, 100).Fraction);
    }

    [Fact]
    public void Text_form_shows_current_over_maximum()
    {
        Assert.Equal("25/100", Health.Of(25, 100).ToString());
    }
}
