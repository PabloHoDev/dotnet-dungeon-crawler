namespace DungeonCrawler.Domain.Abstractions
{
    /// <summary>
    /// Source of randomness for probabilistic rules (critical hits, loot, encounters).
    /// The domain never touches <see cref="System.Random"/> directly, so every
    /// probabilistic rule can be driven deterministically in tests (RNF-011).
    /// </summary>
    public interface IRandomSource
    {
        /// <summary>Returns a value in the range [0.0, 1.0).</summary>
        double NextDouble();

        /// <summary>Returns an integer in the range [minInclusive, maxExclusive).</summary>
        int NextInt(int minInclusive, int maxExclusive);
    }
}
