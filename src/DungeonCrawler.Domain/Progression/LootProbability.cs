using System;
using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Items;

namespace DungeonCrawler.Domain.Progression
{
    /// <summary>
    /// Mathematical Easter Egg #4 — loot probability.
    /// Rarity is chosen from a weighted distribution: 60 + 25 + 10 + 4 + 1 = 100.
    /// Luck moves weight from Common to the better rarities, up to a limit, and
    /// the weights always keep adding up to 100.
    /// </summary>
    public static class LootProbability
    {
        private static readonly double[] BaseWeights = { 60.0, 25.0, 10.0, 4.0, 1.0 };

        /// <summary>Weights in percent, indexed by <see cref="Rarity"/>, for the given Luck.</summary>
        public static IReadOnlyList<double> WeightsFor(int luck)
        {
            Guard.AgainstNegative(luck, nameof(luck));

            var shift = Math.Min(luck * Balance.LuckRarityShiftPerPoint, Balance.MaxLuckRarityShift);
            var nonCommonTotal = 100.0 - BaseWeights[0];

            var weights = new double[BaseWeights.Length];
            weights[0] = BaseWeights[0] - shift;
            for (var i = 1; i < BaseWeights.Length; i++)
            {
                // The shifted weight is shared in proportion to each rarity's base weight.
                weights[i] = BaseWeights[i] + (shift * BaseWeights[i] / nonCommonTotal);
            }

            return weights;
        }

        /// <summary>Picks the rarity that a roll in [0, 1) falls on.</summary>
        public static Rarity Pick(double roll, int luck)
        {
            if (double.IsNaN(roll) || roll < 0.0 || roll >= 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(roll), roll, "A roll must be in the range [0, 1).");
            }

            var weights = WeightsFor(luck);
            var target = roll * 100.0;
            var accumulated = 0.0;

            for (var i = 0; i < weights.Count; i++)
            {
                accumulated += weights[i];
                if (target < accumulated)
                {
                    return (Rarity)i;
                }
            }

            return Rarity.Legendary;
        }
    }
}
