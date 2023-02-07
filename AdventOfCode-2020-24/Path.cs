using System.Drawing;
using static AdventOfCode_2020_24.HexDirectionExtensions;

namespace AdventOfCode_2020_24;

public class Path
{
    private readonly IEnumerable<HexDirection> _directions;

    public Path(IEnumerable<HexDirection> directions)
    {
        _directions = directions;
    }

    public Point EndCoordinate() => _directions.Aggregate(new Point(0, 0), Follow);

    private static Point Follow(Point startPoint, HexDirection direction) =>
        HexDirectionToVector(direction).Add(startPoint);
}