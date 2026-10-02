using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Enemies;
using DungeonCrawler.Domain.Items;
using DungeonCrawler.Domain.Progression;

namespace DungeonCrawler.Domain.Rewards
{
    /// <summary>What an encounter yields: experience, gold and possibly an item (RF-013).</summary>
    public sealed class Reward
    {
        public Reward(int experience, int gold, Equipment? item)
        {
            Experience = Guard.AgainstNegative(experience, nameof(experience));
            Gold = Guard.AgainstNegative(gold, nameof(gold));
            Item = item;
        }

        public int Experience { get; }

        public int Gold { get; }

        public Equipment? Item { get; }

        /// <summary>The reward for defeating an enemy. Harder enemies yield more.</summary>
        public static Reward ForDefeating(Enemy enemy, int floorNumber, int luck, IRandomSource random)
        {
            Guard.AgainstNull(enemy, nameof(enemy));

            return new Reward(
                enemy.ExperienceReward,
                enemy.GoldReward,
                LootGenerator.RollDrop(enemy.Category, floorNumber, luck, random));
        }

        /// <summary>The contents of a treasure room: gold and a guaranteed item.</summary>
        public static Reward ForTreasure(int floorNumber, int luck, IRandomSource random)
        {
            return new Reward(
                0,
                Balance.TreasureGoldPerFloor * floorNumber,
                LootGenerator.Generate(floorNumber, luck, Rarity.Common, random));
        }

        /// <summary>Gives the reward to the character.</summary>
        public GrantedReward GrantTo(Character character)
        {
            Guard.AgainstNull(character, nameof(character));

            character.AddGold(Gold);

            var itemStored = false;
            if (Item is not null && !character.Inventory.IsFull)
            {
                character.Store(Item);
                itemStored = true;
            }

            // Experience goes last: a level up fully restores the character.
            var levelsGained = character.GainExperience(Experience);

            return new GrantedReward(this, levelsGained, itemStored);
        }
    }
}
