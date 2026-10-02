using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Dungeon;
using DungeonCrawler.Domain.Tests.TestDoubles;
using Xunit.Abstractions;

namespace DungeonCrawler.Domain.Tests.Simulation;

/// <summary>
/// Plays hundreds of seeded expeditions to keep the balance inside the intended
/// band: a first expedition must be winnable but far from guaranteed, and a
/// character that keeps its progress must win more often on later attempts.
/// </summary>
public class BalanceSimulationTests
{
    private const int Runs = 300;

    private readonly ITestOutputHelper _output;

    public BalanceSimulationTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [InlineData(ClassId.Warrior)]
    [InlineData(ClassId.Mage)]
    [InlineData(ClassId.Rogue)]
    public void A_first_expedition_is_winnable_but_not_guaranteed(ClassId classId)
    {
        var wins = 0;
        var levels = 0;
        var deathsByFloor = new int[DungeonDefinition.ForgottenCrypt.Floors.Count];

        for (var seed = 0; seed < Runs; seed++)
        {
            var random = new SeededRandomSource(seed);
            var character = Character.Create(Guid.NewGuid(), "Sim", CharacterClass.Get(classId));
            var expedition = Expedition.Start(character, DungeonDefinition.ForgottenCrypt, random);

            if (AutoPlayer.Play(expedition, random) == ExpeditionStatus.Completed)
            {
                wins++;
            }
            else
            {
                deathsByFloor[expedition.CurrentFloor.Number - 1]++;
            }

            levels += character.Level.Value;
        }

        var winRate = (double)wins / Runs;
        _output.WriteLine(
            $"{classId}: first-run win rate {winRate:P0}, average final level {(double)levels / Runs:0.0}, " +
            $"deaths by floor [{string.Join(", ", deathsByFloor)}]");

        Assert.InRange(winRate, 0.25, 0.85);
    }

    [Theory]
    [InlineData(ClassId.Warrior)]
    [InlineData(ClassId.Mage)]
    [InlineData(ClassId.Rogue)]
    public void Permanent_progress_makes_later_expeditions_easier(ClassId classId)
    {
        var firstRunWins = 0;
        var thirdRunWins = 0;

        for (var seed = 0; seed < Runs; seed++)
        {
            var random = new SeededRandomSource(seed);
            var character = Character.Create(Guid.NewGuid(), "Sim", CharacterClass.Get(classId));

            if (AutoPlayer.PlayExpedition(character, random) == ExpeditionStatus.Completed)
            {
                firstRunWins++;
            }

            AutoPlayer.PlayExpedition(character, random);

            if (AutoPlayer.PlayExpedition(character, random) == ExpeditionStatus.Completed)
            {
                thirdRunWins++;
            }
        }

        _output.WriteLine($"{classId}: first run {firstRunWins}/{Runs}, third run {thirdRunWins}/{Runs}");

        Assert.True(
            thirdRunWins >= firstRunWins,
            $"Third run wins ({thirdRunWins}) should not be fewer than first run wins ({firstRunWins}).");
    }
}
