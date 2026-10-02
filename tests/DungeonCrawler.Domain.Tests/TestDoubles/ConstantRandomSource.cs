using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Tests.TestDoubles;

/// <summary>
/// Always returns the same roll. With a high value nothing probabilistic
/// happens (no evasion, no critical, no drop); with a low value everything does.
/// </summary>
public sealed class ConstantRandomSource : IRandomSource
{
    /// <summary>No evasion, no critical hit, no common drop.</summary>
    public static readonly ConstantRandomSource NothingHappens = new(0.99);

    private readonly double _value;

    public ConstantRandomSource(double value)
    {
        _value = value;
    }

    public double NextDouble()
    {
        return _value;
    }

    public int NextInt(int minInclusive, int maxExclusive)
    {
        return minInclusive + (int)(_value * (maxExclusive - minInclusive));
    }
}
