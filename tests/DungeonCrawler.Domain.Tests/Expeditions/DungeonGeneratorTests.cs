using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Dungeon;
using DungeonCrawler.Domain.Enemies;
using DungeonCrawler.Domain.Tests.TestDoubles;

namespace DungeonCrawler.Domain.Tests.Expeditions;

public class DungeonGeneratorTests
{
    private static DungeonLayout Generate(int seed)
    {
        return DungeonGenerator.Generate(DungeonDefinition.ForgottenCrypt, new SeededRandomSource(seed));
    }

    [Fact]
    public void The_dungeon_of_the_MVP_has_five_floors()
    {
        var dungeon = Generate(1);

        Assert.Equal("Forgotten Crypt", dungeon.Name);
        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, dungeon.Floors.Select(floor => floor.Number));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(77)]
    public void The_structure_is_fixed_whatever_the_seed(int seed)
    {
        var dungeon = Generate(seed);

        foreach (var floor in dungeon.Floors)
        {
            Assert.Equal(3, floor.Stages.Count);
            Assert.Equal(new[] { RoomType.Combat }, floor.Stages[0].Rooms.Select(room => room.Type));
            Assert.Equal(new[] { RoomType.Combat, RoomType.Treasure }, floor.Stages[1].Rooms.Select(room => room.Type));
        }

        Assert.Equal(RoomType.Combat, dungeon.Floors[0].Stages[2].Rooms.Single().Type);
        Assert.Equal(RoomType.Combat, dungeon.Floors[1].Stages[2].Rooms.Single().Type);
        Assert.Equal(RoomType.Elite, dungeon.Floors[2].Stages[2].Rooms.Single().Type);
        Assert.Equal(RoomType.MiniBoss, dungeon.Floors[3].Stages[2].Rooms.Single().Type);
        Assert.Equal(RoomType.Boss, dungeon.Floors[4].Stages[2].Rooms.Single().Type);
    }

    [Fact]
    public void The_elite_is_a_common_enemy_with_the_elite_modifier()
    {
        var elite = Generate(1).Floors[2].Stages[2].Rooms.Single();

        Assert.Same(Bestiary.Goblin, elite.Enemy);
        Assert.Same(EnemyModifier.Elite, elite.Modifier);
    }

    [Fact]
    public void The_mini_boss_and_the_boss_are_the_ones_of_the_definition()
    {
        var dungeon = Generate(1);

        Assert.Same(Bestiary.SkeletonKnight, dungeon.Floors[3].Stages[2].Rooms.Single().Enemy);
        Assert.Same(Bestiary.CryptLord, dungeon.Floors[4].Stages[2].Rooms.Single().Enemy);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Combat_rooms_only_hold_enemies_from_the_pool_of_their_floor(int floorNumber)
    {
        var pool = DungeonDefinition.ForgottenCrypt.Floors[floorNumber - 1].CommonEnemies;

        for (var seed = 0; seed < 30; seed++)
        {
            var combatRooms = Generate(seed).Floors[floorNumber - 1].Stages
                .SelectMany(stage => stage.Rooms)
                .Where(room => room.Type == RoomType.Combat);

            Assert.All(combatRooms, room => Assert.Contains(room.Enemy, pool));
        }
    }

    [Fact]
    public void Treasure_rooms_have_no_enemy()
    {
        var treasureRooms = Generate(1).Floors
            .SelectMany(floor => floor.Stages)
            .SelectMany(stage => stage.Rooms)
            .Where(room => room.Type == RoomType.Treasure);

        Assert.All(treasureRooms, room => Assert.False(room.HasEnemy));
    }

    [Fact]
    public void The_same_seed_generates_the_same_dungeon()
    {
        static string[] Describe(DungeonLayout dungeon)
        {
            return dungeon.Floors
                .SelectMany(floor => floor.Stages)
                .SelectMany(stage => stage.Rooms)
                .Select(room => $"{room.Type}:{room.Enemy?.Id}")
                .ToArray();
        }

        Assert.Equal(Describe(Generate(7)), Describe(Generate(7)));
    }

    [Fact]
    public void Different_seeds_vary_the_enemies()
    {
        var variations = Enumerable.Range(0, 30)
            .Select(seed => string.Join(",", Generate(seed).Floors[1].Stages.SelectMany(s => s.Rooms).Select(r => r.Enemy?.Id)))
            .Distinct();

        Assert.True(variations.Count() > 1);
    }

    [Fact]
    public void A_dungeon_without_a_final_boss_room_is_invalid()
    {
        var floors = new[]
        {
            new Floor(1, new[] { new Stage(new[] { Room.WithEnemy(RoomType.Combat, Bestiary.Goblin) }) }),
        };

        Assert.Throws<DomainException>(() => new DungeonLayout("Broken", floors));
    }

    [Fact]
    public void A_boss_room_that_can_be_skipped_is_invalid()
    {
        var floors = new[]
        {
            new Floor(1, new[]
            {
                new Stage(new[] { Room.WithEnemy(RoomType.Boss, Bestiary.CryptLord), Room.Treasure() }),
            }),
        };

        Assert.Throws<DomainException>(() => new DungeonLayout("Broken", floors));
    }

    [Fact]
    public void Floors_and_stages_cannot_be_empty()
    {
        Assert.Throws<DomainException>(() => new Stage(Array.Empty<Room>()));
        Assert.Throws<DomainException>(() => new Floor(1, Array.Empty<Stage>()));
        Assert.Throws<DomainException>(() => new DungeonLayout("Empty", Array.Empty<Floor>()));
    }
}
