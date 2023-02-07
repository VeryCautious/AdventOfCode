using System.Drawing;

namespace AdventOfCode_2020_24;

public enum HexDirection
{
    East,
    SouthEast,
    SouthWest,
    West,
    NorthWest,
    NorthEast
}

public static class HexDirectionExtensions
{
    public static Point HexDirectionToVector(HexDirection direction) => direction switch
    {
        HexDirection.East => new Point(2, 0),
        HexDirection.West => new Point(-2, 0),
        HexDirection.NorthWest => new Point(-1, 1),
        HexDirection.SouthWest => new Point(-1, -1),
        HexDirection.NorthEast => new Point(1, 1),
        HexDirection.SouthEast => new Point(1, -1),
        _ => throw new ArgumentOutOfRangeException(nameof(direction))
    };
}