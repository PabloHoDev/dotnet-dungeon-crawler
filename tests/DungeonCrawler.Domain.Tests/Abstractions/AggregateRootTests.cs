using DungeonCrawler.Domain.Abstractions;

namespace DungeonCrawler.Domain.Tests.Abstractions;

public class AggregateRootTests
{
    [Fact]
    public void A_new_aggregate_has_no_pending_events()
    {
        var chest = new Chest(Guid.NewGuid());

        Assert.Empty(chest.DomainEvents);
    }

    [Fact]
    public void Operations_record_their_events_in_order()
    {
        var chest = new Chest(Guid.NewGuid());

        chest.Open();
        chest.Close();

        Assert.Collection(
            chest.DomainEvents,
            first => Assert.IsType<ChestOpened>(first),
            second => Assert.IsType<ChestClosed>(second));
    }

    [Fact]
    public void Dequeuing_returns_the_pending_events_and_clears_them()
    {
        var chest = new Chest(Guid.NewGuid());
        chest.Open();

        var dequeued = chest.DequeueDomainEvents();

        Assert.Single(dequeued);
        Assert.Empty(chest.DomainEvents);
        Assert.Empty(chest.DequeueDomainEvents());
    }

    private sealed class Chest : AggregateRoot<Guid>
    {
        public Chest(Guid id)
            : base(id)
        {
        }

        public void Open()
        {
            Raise(new ChestOpened());
        }

        public void Close()
        {
            Raise(new ChestClosed());
        }
    }

    private sealed class ChestOpened : IDomainEvent
    {
    }

    private sealed class ChestClosed : IDomainEvent
    {
    }
}
