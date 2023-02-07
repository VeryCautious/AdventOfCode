using System.Collections.Immutable;

namespace AdventOfCode_2020_24;

public static class HexDirectionParser
{

    public static IImmutableList<HexDirection> Parse(string inputLine)
    {
        var charQueue = new Queue<char>(inputLine.ToCharArray());
        var directions = new List<HexDirection>();

        var str = string.Empty;

        while (charQueue.Any())
        {
            str += charQueue.Dequeue();

            if (IsHexDirection(str))
            {
                directions.Add(ParseSingle(str));
                str = string.Empty;
            }
        }

        return directions.ToImmutableList();
    }

    private static bool IsHexDirection(string str) => str is "e" or "w" or "ne" or "nw" or "sw" or "se";
    private static HexDirection ParseSingle(string str) => str switch
    {
        "e" => HexDirection.East,
        "w" => HexDirection.West,
        "ne" => HexDirection.NorthEast,
        "nw" => HexDirection.NorthWest,
        "se" => HexDirection.SouthEast,
        "sw" => HexDirection.SouthWest,
        _ => throw new ArgumentOutOfRangeException(nameof(str))
    };

}