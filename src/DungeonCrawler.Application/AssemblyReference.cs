using System.Reflection;

namespace DungeonCrawler.Application
{
    /// <summary>
    /// Stable handle to the Application assembly, used by the architecture tests.
    /// Use cases and ports are added from ETAPA 04 onwards.
    /// </summary>
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
