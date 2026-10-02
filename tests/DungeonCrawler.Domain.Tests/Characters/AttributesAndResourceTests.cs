using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;

namespace DungeonCrawler.Domain.Tests.Characters;

public class AttributesTests
{
    private static readonly Attributes Sample = new(strength: 8, vitality: 7, agility: 4, intelligence: 2, luck: 3);

    [Fact]
    public void Each_attribute_can_be_read_by_type()
    {
        Assert.Equal(8, Sample.Get(AttributeType.Strength));
        Assert.Equal(7, Sample.Get(AttributeType.Vitality));
        Assert.Equal(4, Sample.Get(AttributeType.Agility));
        Assert.Equal(2, Sample.Get(AttributeType.Intelligence));
        Assert.Equal(3, Sample.Get(AttributeType.Luck));
        Assert.Equal(24, Sample.Total);
    }

    [Theory]
    [InlineData(AttributeType.Strength)]
    [InlineData(AttributeType.Vitality)]
    [InlineData(AttributeType.Agility)]
    [InlineData(AttributeType.Intelligence)]
    [InlineData(AttributeType.Luck)]
    public void Increasing_one_attribute_leaves_the_others_untouched(AttributeType type)
    {
        var increased = Sample.Increase(type, 5);

        Assert.Equal(Sample.Get(type) + 5, increased.Get(type));
        Assert.Equal(Sample.Total + 5, increased.Total);
    }

    [Fact]
    public void Increasing_returns_a_new_set_and_keeps_the_original()
    {
        Sample.Increase(AttributeType.Strength, 5);

        Assert.Equal(8, Sample.Strength);
    }

    [Fact]
    public void Two_sets_can_be_added_together()
    {
        var bonus = Attributes.Of(AttributeType.Luck, 4);

        Assert.Equal(new Attributes(8, 7, 4, 2, 7), Sample.Plus(bonus));
    }

    [Fact]
    public void Negative_values_are_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Attributes(-1, 0, 0, 0, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Sample.Increase(AttributeType.Luck, -1));
    }
}

public class ResourceTests
{
    [Fact]
    public void A_full_pool_starts_at_the_maximum()
    {
        Assert.Equal(Resource.Of(30, 30), Resource.Full(30));
    }

    [Fact]
    public void Spending_reduces_the_pool()
    {
        Assert.Equal(Resource.Of(20, 30), Resource.Full(30).Spend(10));
    }

    [Fact]
    public void Spending_more_than_available_is_not_allowed()
    {
        var pool = Resource.Of(5, 30);

        Assert.False(pool.CanAfford(6));
        Assert.True(pool.CanAfford(5));
        Assert.Throws<DomainException>(() => pool.Spend(6));
    }

    [Fact]
    public void Restoring_never_goes_above_the_maximum()
    {
        Assert.Equal(Resource.Of(15, 30), Resource.Of(10, 30).Restore(5));
        Assert.Equal(Resource.Full(30), Resource.Of(28, 30).Restore(50));
        Assert.Equal(Resource.Full(30), Resource.Of(0, 30).Refill());
    }

    [Fact]
    public void Changing_the_maximum_lowers_the_current_value_when_needed()
    {
        Assert.Equal(Resource.Of(20, 20), Resource.Of(25, 30).WithMaximum(20));
        Assert.Equal(Resource.Of(25, 40), Resource.Of(25, 30).WithMaximum(40));
    }
}
