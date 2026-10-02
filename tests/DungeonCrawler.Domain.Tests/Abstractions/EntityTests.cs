using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Tests.Abstractions;

public class EntityTests
{
    [Fact]
    public void Entities_with_the_same_identity_are_equal_regardless_of_state()
    {
        var id = Guid.NewGuid();
        var a = new Hero(id, "Aria");
        var b = new Hero(id, "Borin");

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Entities_with_different_identities_are_not_equal_even_with_the_same_state()
    {
        var a = new Hero(Guid.NewGuid(), "Aria");
        var b = new Hero(Guid.NewGuid(), "Aria");

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    [Fact]
    public void Entities_of_different_types_are_not_equal_even_with_the_same_identity()
    {
        var id = Guid.NewGuid();
        Entity<Guid> hero = new Hero(id, "Aria");
        Entity<Guid> monster = new Monster(id);

        Assert.NotEqual(hero, monster);
    }

    [Fact]
    public void An_entity_cannot_be_created_with_a_default_identity()
    {
        Assert.Throws<ArgumentException>(() => new Hero(Guid.Empty, "Aria"));
    }

    [Fact]
    public void A_satisfied_rule_lets_the_operation_proceed()
    {
        var hero = new Hero(Guid.NewGuid(), "Aria");

        hero.Rename("Borin");

        Assert.Equal("Borin", hero.Name);
    }

    [Fact]
    public void A_broken_rule_stops_the_operation_and_reports_the_rule()
    {
        var hero = new Hero(Guid.NewGuid(), "Aria");

        var exception = Assert.Throws<BusinessRuleViolationException>(() => hero.Rename(" "));

        Assert.IsType<NameCannotBeBlankRule>(exception.BrokenRule);
        Assert.Equal("A hero needs a name.", exception.Message);
        Assert.Equal("Aria", hero.Name);
    }

    private sealed class Hero : Entity<Guid>
    {
        public Hero(Guid id, string name)
            : base(id)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public void Rename(string name)
        {
            CheckRule(new NameCannotBeBlankRule(name));
            Name = name;
        }
    }

    private sealed class Monster : Entity<Guid>
    {
        public Monster(Guid id)
            : base(id)
        {
        }
    }

    private sealed class NameCannotBeBlankRule : IBusinessRule
    {
        private readonly string _name;

        public NameCannotBeBlankRule(string name)
        {
            _name = name;
        }

        public string Message => "A hero needs a name.";

        public bool IsBroken()
        {
            return string.IsNullOrWhiteSpace(_name);
        }
    }
}
