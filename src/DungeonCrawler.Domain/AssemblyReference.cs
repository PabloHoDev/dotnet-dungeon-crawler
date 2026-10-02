using System.Reflection;

namespace DungeonCrawler.Domain
{
    /// <summary>
    /// Stable handle to the Domain assembly, used by the architecture tests.
    /// </summary>
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
