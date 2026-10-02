using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Progression
{
    /// <summary>
    /// Mathematical Easter Egg #1 — the experience curve.
    /// XP(n) = 50 × n × (n + 1) is the accumulated experience needed to complete
    /// level n. Because n × (n + 1) / 2 is the n-th triangular number, each level
    /// costs exactly 100 × n more experience than nothing: 100, 200, 300, ...
    /// </summary>
    public static class ExperienceCurve
    {
        /// <summary>Accumulated experience at which the maximum level is reached.</summary>
        public static readonly int MaxTotal = TotalToReach(Level.Max);

        /// <summary>XP(n): accumulated experience needed to complete the given level.</summary>
        public static int TotalToComplete(Level level)
        {
            Guard.AgainstNull(level, nameof(level));
            return Balance.ExperienceCurveFactor * level.Value * (level.Value + 1);
        }

        /// <summary>Accumulated experience at which the given level starts.</summary>
        public static int TotalToReach(Level level)
        {
            Guard.AgainstNull(level, nameof(level));
            return Balance.ExperienceCurveFactor * (level.Value - 1) * level.Value;
        }

        /// <summary>Experience needed to go from the start of the level to the next one.</summary>
        public static int CostOf(Level level)
        {
            return TotalToComplete(level) - TotalToReach(level);
        }

        /// <summary>The level a character with the given accumulated experience is at.</summary>
        public static Level LevelFor(int totalExperience)
        {
            Guard.AgainstNegative(totalExperience, nameof(totalExperience));

            var level = Level.First;
            while (!level.IsMax && totalExperience >= TotalToComplete(level))
            {
                level = level.Next();
            }

            return level;
        }

        /// <summary>Experience still missing to reach the next level; zero at the maximum level.</summary>
        public static int RemainingToNextLevel(int totalExperience)
        {
            var level = LevelFor(totalExperience);
            return level.IsMax ? 0 : TotalToComplete(level) - totalExperience;
        }

        /// <summary>Progress inside the current level as a fraction in [0, 1].</summary>
        public static double ProgressInLevel(int totalExperience)
        {
            var level = LevelFor(totalExperience);
            if (level.IsMax)
            {
                return 1.0;
            }

            return (double)(totalExperience - TotalToReach(level)) / CostOf(level);
        }
    }
}
