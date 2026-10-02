using System;

namespace DungeonCrawler.Domain.Abstractions
{
    /// <summary>
    /// Base type for every failure raised by the domain when an operation
    /// is not allowed in the current state (RNF-014).
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message)
            : base(message)
        {
        }
    }
}
