using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Tests.TestDoubles;

/// <summary>
/// Deterministic <see cref="IRandomSource"/> that replays a scripted sequence of rolls.
/// </summary>
public sealed class FixedRandomSource : IRandomSource
{
    private readonly Queue<double> _rolls;

    public FixedRandomSource(params double[] rolls)
    {
        _rolls = new Queue<double>(rolls);
    }

    public double NextDouble()
    {
        if (_rolls.Count == 0)
        {
            throw new InvalidOperationException("The scripted sequence of rolls is exhausted.");
        }

        return _rolls.Dequeue();
    }

    public int NextInt(int minInclusive, int maxExclusive)
    {
        return minInclusive + (int)(NextDouble() * (maxExclusive - minInclusive));
    }
}
