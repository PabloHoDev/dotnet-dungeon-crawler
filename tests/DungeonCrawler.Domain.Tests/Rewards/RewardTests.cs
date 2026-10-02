using DungeonCrawler.Domain.Enemies;
using DungeonCrawler.Domain.Items;
using DungeonCrawler.Domain.Progression;
using DungeonCrawler.Domain.Rewards;
using DungeonCrawler.Domain.SharedKernel;
using DungeonCrawler.Domain.Tests.TestDoubles;

namespace DungeonCrawler.Domain.Tests.Rewards;

public class RewardTests
{
    [Fact]
    public void Defeating_an_enemy_yields_its_experience_and_gold()
    {
        var goblin = Make.Enemy(Bestiary.Goblin);

        var reward = Reward.ForDefeating(goblin, 1, 0, ConstantRandomSource.NothingHappens);

        Assert.Equal(goblin.ExperienceReward, reward.Experience);
        Assert.Equal(goblin.GoldReward, reward.Gold);
        Assert.Null(reward.Item);
    }

    [Fact]
    public void Harder_enemies_yield_better_rewards()
    {
        var random = ConstantRandomSource.NothingHappens;
        var common = Reward.ForDefeating(Make.Enemy(Bestiary.Goblin), 1, 0, random);
        var elite = Reward.ForDefeating(EnemyFactory.Create(Bestiary.Goblin, EnemyModifier.Elite, Level.First, 1), 1, 0, random);
        var miniBoss = Reward.ForDefeating(Make.Enemy(Bestiary.SkeletonKnight), 1, 0, random);
        var boss = Reward.ForDefeating(Make.Enemy(Bestiary.CryptLord), 1, 0, random);

        Assert.True(common.Experience < elite.Experience);
        Assert.True(elite.Experience < miniBoss.Experience);
        Assert.True(miniBoss.Experience < boss.Experience);

        Assert.True(common.Gold < elite.Gold && elite.Gold < miniBoss.Gold && miniBoss.Gold < boss.Gold);

        Assert.Null(common.Item);
        Assert.True(elite.Item!.Rarity >= Rarity.Uncommon);
        Assert.True(miniBoss.Item!.Rarity >= Rarity.Rare);
        Assert.True(boss.Item!.Rarity >= Rarity.Epic);
    }

    [Fact]
    public void A_treasure_holds_gold_proportional_to_the_floor_and_always_an_item()
    {
        var reward = Reward.ForTreasure(3, 0, new SeededRandomSource(1));

        Assert.Equal(0, reward.Experience);
        Assert.Equal(3 * Balance.TreasureGoldPerFloor, reward.Gold);
        Assert.NotNull(reward.Item);
    }

    [Fact]
    public void Granting_a_reward_gives_everything_to_the_character()
    {
        var character = Make.Warrior();
        var sword = Make.Weapon(5);

        var granted = new Reward(experience: 100, gold: 30, sword).GrantTo(character);

        Assert.Equal(100, character.Experience);
        Assert.Equal(30, character.Gold);
        Assert.Same(sword, Assert.Single(character.Inventory.Items));
        Assert.Equal(1, granted.LevelsGained);
        Assert.True(granted.ItemStored);
        Assert.False(granted.ItemWasLost);
    }

    [Fact]
    public void An_item_that_does_not_fit_in_the_inventory_is_lost_but_the_rest_is_granted()
    {
        var character = Make.Warrior();
        while (!character.Inventory.IsFull)
        {
            character.Store(Make.Weapon(1));
        }

        var granted = new Reward(experience: 50, gold: 10, Make.Weapon(9)).GrantTo(character);

        Assert.True(granted.ItemWasLost);
        Assert.Equal(Balance.InventoryCapacity, character.Inventory.Count);
        Assert.Equal(50, character.Experience);
        Assert.Equal(10, character.Gold);
    }

    [Fact]
    public void A_reward_without_an_item_loses_nothing()
    {
        var granted = new Reward(experience: 10, gold: 5, item: null).GrantTo(Make.Warrior());

        Assert.False(granted.ItemStored);
        Assert.False(granted.ItemWasLost);
    }
}
