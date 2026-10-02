using System.Reflection;

namespace DungeonCrawler.Infrastructure
{
    /// <summary>
    /// Stable handle to the Infrastructure assembly, used by the architecture tests.
    /// </summary>
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
