using System;
using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Infrastructure.Randomness
{
    /// <summary>
    /// <see cref="IRandomSource"/> backed by <see cref="Random"/>.
    /// Passing a seed makes the whole sequence reproducible (RNF-011).
    /// </summary>
    public sealed class SystemRandomSource : IRandomSource
    {
        private readonly Random _random;

        public SystemRandomSource()
        {
            _random = new Random();
        }

        public SystemRandomSource(int seed)
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
}
