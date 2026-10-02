using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Combat;
using DungeonCrawler.Domain.Dungeon;
using DungeonCrawler.Domain.SharedKernel;
using DungeonCrawler.Infrastructure.Randomness;

namespace DungeonCrawler.Application.Tests;

public class GameSessionTests
{
    private static GameSession NewSession(int seed = 1)
    {
        return new GameSession(new SystemRandomSource(seed));
    }

    private static TurnOutcome FightUntilTheCombatEnds(GameSession session)
    {
        TurnOutcome outcome;
        do
        {
            outcome = session.TakeTurn(PlayerAction.BasicAttack());
        }
        while (outcome.Round.Status == CombatStatus.InProgress);

        return outcome;
    }

    [Fact]
    public void A_new_session_has_no_character_and_no_expedition()
    {
        var session = NewSession();

        Assert.Null(session.Character);
        Assert.Null(session.Expedition);
        Assert.False(session.HasExpeditionInProgress);
    }

    [Theory]
    [InlineData(ClassId.Warrior)]
    [InlineData(ClassId.Mage)]
    [InlineData(ClassId.Rogue)]
    public void The_player_creates_a_character_of_the_chosen_class(ClassId classId)
    {
        var session = NewSession();

        var character = session.CreateCharacter("Aria", classId);

        Assert.Same(character, session.Character);
        Assert.Equal("Aria", character.Name);
        Assert.Equal(classId, character.Class.Id);
        Assert.Equal(Level.First, character.Level);
    }

    [Fact]
    public void An_expedition_cannot_start_without_a_character()
    {
        Assert.Throws<DomainException>(() => NewSession().StartExpedition());
    }

    [Fact]
    public void Starting_an_expedition_puts_the_character_in_the_dungeon()
    {
        var session = NewSession();
        var character = session.CreateCharacter("Aria", ClassId.Warrior);

        var expedition = session.StartExpedition();

        Assert.Same(character, expedition.Character);
        Assert.Same(expedition, session.Expedition);
        Assert.True(session.HasExpeditionInProgress);
        Assert.Equal(5, expedition.Dungeon.Floors.Count);
    }

    [Fact]
    public void Only_one_expedition_can_be_in_progress()
    {
        var session = NewSession();
        session.CreateCharacter("Aria", ClassId.Warrior);
        session.StartExpedition();

        Assert.Throws<DomainException>(() => session.StartExpedition());
        Assert.Throws<DomainException>(() => session.CreateCharacter("Other", ClassId.Mage));
    }

    [Fact]
    public void Rooms_and_turns_require_an_expedition()
    {
        var session = NewSession();
        session.CreateCharacter("Aria", ClassId.Warrior);

        Assert.Throws<DomainException>(() => session.EnterRoom(0));
        Assert.Throws<DomainException>(() => session.TakeTurn(PlayerAction.BasicAttack()));
    }

    [Fact]
    public void The_player_enters_a_room_fights_and_is_rewarded()
    {
        var session = NewSession();
        var character = session.CreateCharacter("Aria", ClassId.Warrior);
        session.StartExpedition();

        var entry = session.EnterRoom(0);
        var outcome = FightUntilTheCombatEnds(session);

        Assert.NotNull(entry.StartedCombat);
        Assert.Equal(CombatStatus.Victory, outcome.Round.Status);
        Assert.True(character.Experience > 0);
        Assert.True(character.Gold > 0);
    }

    [Fact]
    public void After_dying_the_same_character_starts_a_new_expedition_with_its_progress()
    {
        var session = NewSession();
        var character = session.CreateCharacter("Aria", ClassId.Mage);
        session.StartExpedition();
        session.EnterRoom(0);
        FightUntilTheCombatEnds(session);
        var experience = character.Experience;

        session.EnterRoom(0);
        character.TakeDamage(character.Health.Current - 1);
        while (session.HasExpeditionInProgress)
        {
            session.TakeTurn(PlayerAction.BasicAttack());
        }

        Assert.Equal(ExpeditionStatus.Failed, session.Expedition!.Status);

        var next = session.StartExpedition();

        Assert.Same(character, next.Character);
        Assert.Equal(experience, character.Experience);
        Assert.True(character.Health.IsFull);
        Assert.Equal(ExpeditionStatus.InProgress, next.Status);
    }

    [Fact]
    public void The_same_seed_produces_the_same_session()
    {
        static (int Experience, int Gold, int Health) Play(int seed)
        {
            var session = NewSession(seed);
            var character = session.CreateCharacter("Aria", ClassId.Rogue);
            session.StartExpedition();
            session.EnterRoom(0);
            FightUntilTheCombatEnds(session);
            return (character.Experience, character.Gold, character.Health.Current);
        }

        Assert.Equal(Play(21), Play(21));
    }
}
