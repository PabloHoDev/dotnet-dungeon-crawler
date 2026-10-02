using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Dungeon;
using DungeonCrawler.Domain.Enemies;
using DungeonCrawler.Domain.SharedKernel;
using DungeonCrawler.Domain.Tests.TestDoubles;

namespace DungeonCrawler.Domain.Tests.Simulation;

/// <summary>
/// RF-043 — the Golden Path of the MVP, played from start to finish with a
/// brand-new Warrior: create character, enter the dungeon, fight, earn
/// experience, level up, equip an item, advance through the floors, beat the
/// elite, the mini boss and the boss, and complete the expedition.
/// </summary>
public class GoldenPathTests
{
    [Fact]
    public void A_new_warrior_can_play_the_whole_golden_path()
    {
        var run = FirstWinningRun();

        // Create character and choose class.
        Assert.Equal(ClassId.Warrior, run.Character.Class.Id);

        // Enter the dungeon, explore and advance through all five floors.
        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, run.FloorsVisited);

        // Fight: the first enemy is the Goblin of floor 1.
        Assert.Equal("Goblin", run.EnemiesDefeated[0].Name);
        Assert.Contains(run.EnemiesDefeated, enemy => enemy.Definition == Bestiary.Skeleton);

        // Receive experience and level up.
        Assert.True(run.Character.Experience > 0);
        Assert.True(run.Character.Level.CompareTo(Level.First) > 0);
        Assert.True(run.LeveledUpOnFloor1, "The first level up should happen on floor 1.");

        // Equip an item.
        Assert.NotEmpty(run.Character.Loadout.Equipped);

        // Elite, Mini Boss, Boss — in that order.
        var special = run.EnemiesDefeated
            .Where(enemy => enemy.Category != EnemyCategory.Common)
            .Select(enemy => enemy.Category);
        Assert.Equal(new[] { EnemyCategory.Elite, EnemyCategory.MiniBoss, EnemyCategory.Boss }, special);
        Assert.Same(Bestiary.CryptLord, run.EnemiesDefeated[^1].Definition);

        // Victory.
        Assert.Equal(ExpeditionStatus.Completed, run.Expedition.Status);
    }

    private static Run FirstWinningRun()
    {
        for (var seed = 0; seed < 50; seed++)
        {
            var run = Play(seed);
            if (run.Expedition.Status == ExpeditionStatus.Completed)
            {
                return run;
            }
        }

        throw new InvalidOperationException("No seed in 0..49 let a new Warrior complete the expedition.");
    }

    private static Run Play(int seed)
    {
        var random = new SeededRandomSource(seed);
        var character = Character.Create(Guid.NewGuid(), "Aria", CharacterClass.Warrior);
        var expedition = Expedition.Start(character, DungeonDefinition.ForgottenCrypt, random);
        var run = new Run(character, expedition);

        while (expedition.Status == ExpeditionStatus.InProgress)
        {
            AutoPlayer.Prepare(character);
            run.FloorsVisited.Add(expedition.CurrentFloor.Number);

            var floor = expedition.CurrentFloor.Number;
            var combat = expedition.EnterRoom(0, random).StartedCombat;

            while (expedition.IsInCombat)
            {
                var outcome = expedition.TakeTurn(PlayerAction.BasicAttack(), random);
                if (outcome.Round.Status == CombatStatus.Victory)
                {
                    run.EnemiesDefeated.Add(combat!.Enemy);
                    run.LeveledUpOnFloor1 |= floor == 1 && outcome.Reward!.LevelsGained > 0;
                }
            }
        }

        return run;
    }

    private sealed class Run
    {
        public Run(Character character, Expedition expedition)
        {
            Character = character;
            Expedition = expedition;
        }

        public Character Character { get; }

        public Expedition Expedition { get; }

        public SortedSet<int> FloorsVisited { get; } = new();

        public List<Enemy> EnemiesDefeated { get; } = new();

        public bool LeveledUpOnFloor1 { get; set; }
    }
}
