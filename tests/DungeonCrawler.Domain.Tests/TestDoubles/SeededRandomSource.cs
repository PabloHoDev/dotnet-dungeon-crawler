using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Tests.TestDoubles;

/// <summary>Pseudo-random but reproducible: the same seed always yields the same sequence.</summary>
public sealed class SeededRandomSource : IRandomSource
{
    private readonly Random _random;

    public SeededRandomSource(int seed)
    {
        _random = new Random(seed);
    }

    public double NextDouble()
    {
        return _random.NextDouble();
    }

    public int NextInt(int minInclusive, int maxExclusive)
    {
        return _random.Next(minInclusive, maxExclusive);
    }
}
