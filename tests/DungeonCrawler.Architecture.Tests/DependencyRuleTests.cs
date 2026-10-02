using System.Reflection;
using System.Xml.Linq;

namespace DungeonCrawler.Architecture.Tests;

/// <summary>
/// Enforces the dependency rule described in docs/architecture/ARCHITECTURE.md:
/// Infrastructure -> Application -> Domain, and nothing in the core knows Unity.
/// </summary>
public class DependencyRuleTests
{
    private const string DomainName = "DungeonCrawler.Domain";
    private const string ApplicationName = "DungeonCrawler.Application";
    private const string InfrastructureName = "DungeonCrawler.Infrastructure";

    private static readonly Assembly Domain = DungeonCrawler.Domain.AssemblyReference.Assembly;
    private static readonly Assembly Application = DungeonCrawler.Application.AssemblyReference.Assembly;
    private static readonly Assembly Infrastructure = DungeonCrawler.Infrastructure.AssemblyReference.Assembly;

    public static TheoryData<string> CoreProjects => new() { DomainName, ApplicationName, InfrastructureName };

    [Fact]
    public void Domain_depends_only_on_the_base_class_library()
    {
        Assert.Equal(new[] { "netstandard" }, ReferencedAssemblies(Domain));
    }

    [Fact]
    public void Domain_project_declares_no_project_or_package_references()
    {
        Assert.Empty(DeclaredReferences(DomainName, "ProjectReference"));
        Assert.Empty(DeclaredReferences(DomainName, "PackageReference"));
    }

    [Fact]
    public void Application_depends_only_on_Domain()
    {
        Assert.DoesNotContain(InfrastructureName, ReferencedAssemblies(Application));
        Assert.Equal(new[] { DomainName }, DeclaredReferences(ApplicationName, "ProjectReference"));
    }

    [Fact]
    public void Infrastructure_depends_only_on_Application_and_Domain()
    {
        Assert.Equal(
            new[] { ApplicationName, DomainName },
            DeclaredReferences(InfrastructureName, "ProjectReference"));
    }

    [Theory]
    [MemberData(nameof(CoreProjects))]
    public void Core_projects_do_not_reference_Unity(string projectName)
    {
        var assembly = Assembly.Load(projectName);

        Assert.DoesNotContain(ReferencedAssemblies(assembly), name => name.StartsWith("Unity", StringComparison.Ordinal));
    }

    [Theory]
    [MemberData(nameof(CoreProjects))]
    public void Core_projects_target_netstandard21_so_Unity_can_consume_them(string projectName)
    {
        var assembly = Assembly.Load(projectName);

        Assert.Contains("netstandard", ReferencedAssemblies(assembly));
        Assert.Equal(new Version(2, 1, 0, 0), assembly.GetReferencedAssemblies().Single(a => a.Name == "netstandard").Version);
    }

    private static string[] ReferencedAssemblies(Assembly assembly)
    {
        return assembly.GetReferencedAssemblies()
            .Select(reference => reference.Name!)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
    }

    /// <summary>Names of the references of the given kind declared in the project file.</summary>
    private static string[] DeclaredReferences(string projectName, string itemName)
    {
        var projectFile = Path.Combine(RepositoryRoot(), "src", projectName, projectName + ".csproj");

        return XDocument.Load(projectFile)
            .Descendants(itemName)
            .Select(item => Path.GetFileNameWithoutExtension(item.Attribute("Include")!.Value.Replace('\\', '/')))
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
    }

    private static string RepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "DungeonCrawler.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new InvalidOperationException("Could not locate DungeonCrawler.sln above the test output directory.");
    }
}
