namespace DungeonCrawler.Cli;

/// <summary>Raised when there is no more input to read.</summary>
public sealed class EndOfInputException : Exception
{
}

/// <summary>Small helpers over the console: titles, prompts and validated choices.</summary>
public sealed class ConsoleUi
{
    public void Line(string text = "")
    {
        Console.WriteLine(text);
    }

    public void Title(string text)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 60));
        Console.WriteLine($" {text}");
        Console.WriteLine(new string('=', 60));
    }

    public void Section(string text)
    {
        Console.WriteLine();
        Console.WriteLine($"--- {text} ---");
    }

    public string Ask(string prompt)
    {
        Console.Write($"{prompt} ");
        var answer = Console.ReadLine();
        if (answer is null)
        {
            throw new EndOfInputException();
        }

        // A byte order mark can come first when input is piped from a file or another program.
        return answer.Trim('﻿').Trim();
    }

    /// <summary>Keeps asking until the answer is one of the accepted options (case-insensitive).</summary>
    public string Choose(string prompt, IReadOnlyCollection<string> options)
    {
        while (true)
        {
            var answer = Ask(prompt).ToLowerInvariant();
            if (options.Contains(answer))
            {
                return answer;
            }

            Console.WriteLine($"Opção inválida. Escolha entre: {string.Join(", ", options)}");
        }
    }

    /// <summary>Keeps asking until the answer is a whole number inside the range.</summary>
    public int AskNumber(string prompt, int minimum, int maximum)
    {
        while (true)
        {
            if (int.TryParse(Ask(prompt), out var number) && number >= minimum && number <= maximum)
            {
                return number;
            }

            Console.WriteLine($"Digite um número entre {minimum} e {maximum}.");
        }
    }

    public void Pause()
    {
        Ask("(Enter para continuar)");
    }

    public static string Bar(int current, int maximum, int width = 20)
    {
        var filled = maximum <= 0 ? 0 : (int)Math.Round((double)current / maximum * width);
        filled = Math.Clamp(filled, current > 0 ? 1 : 0, width);
        return "[" + new string('#', filled) + new string('.', width - filled) + "]";
    }
}
