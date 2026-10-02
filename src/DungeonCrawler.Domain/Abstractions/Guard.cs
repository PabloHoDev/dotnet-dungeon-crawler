using System;

namespace DungeonCrawler.Domain.Abstractions
{
    /// <summary>
    /// Argument validation helpers. Invalid arguments are programming errors and
    /// raise <see cref="ArgumentException"/> family exceptions; state-dependent
    /// game rules are expressed as <see cref="IBusinessRule"/> instead.
    /// </summary>
    public static class Guard
    {
        public static T AgainstNull<T>(T? value, string paramName)
            where T : class
        {
            if (value is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return value;
        }

        public static string AgainstNullOrWhiteSpace(string? value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null, empty or whitespace.", paramName);
            }

            return value!;
        }

        public static int AgainstNegative(int value, string paramName)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, value, "Value cannot be negative.");
            }

            return value;
        }

        public static int AgainstOutOfRange(int value, int minInclusive, int maxInclusive, string paramName)
        {
            if (value < minInclusive || value > maxInclusive)
            {
                throw new ArgumentOutOfRangeException(
                    paramName,
                    value,
                    $"Value must be between {minInclusive} and {maxInclusive}.");
            }

            return value;
        }
    }
}
