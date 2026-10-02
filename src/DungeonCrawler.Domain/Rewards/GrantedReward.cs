namespace DungeonCrawler.Domain.Rewards
{
    /// <summary>A reward after being given to a character, with its consequences.</summary>
    public sealed class GrantedReward
    {
        public GrantedReward(Reward reward, int levelsGained, bool itemStored)
        {
            Reward = reward;
            LevelsGained = levelsGained;
            ItemStored = itemStored;
        }

        public Reward Reward { get; }

        public int LevelsGained { get; }

        /// <summary>False when there was an item but the inventory had no room for it.</summary>
        public bool ItemStored { get; }

        public bool ItemWasLost
        {
            get { return Reward.Item is not null && !ItemStored; }
        }
    }
}
