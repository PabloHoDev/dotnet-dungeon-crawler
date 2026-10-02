namespace DungeonCrawler.Domain.Abstractions
{
    /// <summary>
    /// A state-dependent rule of the game (e.g. "a defeated character cannot act").
    /// Rules are small objects so they can be named, reused and tested in isolation.
    /// </summary>
    public interface IBusinessRule
    {
        /// <summary>Explanation shown when the rule is broken.</summary>
        string Message { get; }

        bool IsBroken();
    }
}
