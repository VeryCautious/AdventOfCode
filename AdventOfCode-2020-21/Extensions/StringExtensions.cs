namespace AdventOfCode_2020_21.Extensions;

internal static class StringExtensions
{
    public static string Clear(this string value, IEnumerable<char> removableChars) =>
        removableChars.Aggregate(value, (s, c) => s.Replace(c.ToString(), ""));
}
