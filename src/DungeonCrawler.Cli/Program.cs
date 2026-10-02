using System.Text;
using DungeonCrawler.Application;
using DungeonCrawler.Cli;
using DungeonCrawler.Infrastructure.Randomness;

// Composition root of the terminal version: the only place that knows every layer.
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var seedIndex = Array.IndexOf(args, "--seed");
var random = seedIndex >= 0 && seedIndex + 1 < args.Length && int.TryParse(args[seedIndex + 1], out var seed)
    ? new SystemRandomSource(seed)
    : new SystemRandomSource();

try
{
    new GameApp(new GameSession(random), new ConsoleUi()).Run();
}
catch (EndOfInputException)
{
    // Input was closed (Ctrl+Z / piped input ended): leave quietly.
}
