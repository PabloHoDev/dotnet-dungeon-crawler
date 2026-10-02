using System;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.SharedKernel;

namespace DungeonCrawler.Domain.Progression
{
    /// <summary>
    /// Mathematical Easter Egg #6 — enemy scaling.
    /// EnemyPower = BasePower × LevelMultiplier × FloorMultiplier, so difficulty
    /// follows the player and the depth without hand-written values per floor.
    /// </summary>
    public static class EnemyScaling
    {
        public static double LevelMultiplier(Level playerLevel)
        {
            Guard.AgainstNull(playerLevel, nameof(playerLevel));
            return 1.0 + (Balance.EnemyLevelScalingStep * (playerLevel.Value - 1));
        }

        public static double FloorMultiplier(int floorNumber)
        {
            if (floorNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(floorNumber), floorNumber, "Floors start at 1.");
            }

            return 1.0 + (Balance.EnemyFloorScalingStep * (floorNumber - 1));
        }

        public static double Multiplier(Level playerLevel, int floorNumber)
        {
            return LevelMultiplier(playerLevel) * FloorMultiplier(floorNumber);
        }

        public static int Scale(int baseValue, double multiplier)
        {
            return (int)Math.Round(baseValue * multiplier, MidpointRounding.AwayFromZero);
        }
    }
}
