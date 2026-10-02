using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Tests.Abstractions;

public class ValueObjectTests
{
    [Fact]
    public void Objects_with_the_same_components_are_equal()
    {
        var a = new Coordinate(1, 2);
        var b = new Coordinate(1, 2);

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.False(a != b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Objects_with_different_components_are_not_equal()
    {
        var a = new Coordinate(1, 2);
        var b = new Coordinate(2, 1);

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    [Fact]
    public void Objects_of_different_types_are_not_equal_even_with_the_same_components()
    {
        ValueObject coordinate = new Coordinate(1, 2);
        ValueObject size = new Size(1, 2);

        Assert.NotEqual(coordinate, size);
    }

    [Fact]
    public void An_object_is_never_equal_to_null()
    {
        var a = new Coordinate(1, 2);
        Coordinate? none = null;

        Assert.False(a.Equals(none));
        Assert.False(a == none);
        Assert.True(none == null);
    }

    private sealed class Coordinate : ValueObject
    {
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return X;
            yield return Y;
        }
    }

    private sealed class Size : ValueObject
    {
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }

        public int Height { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Width;
            yield return Height;
        }
    }
}
