using System.Drawing;
using static AdventOfCode_2020_24.HexDirectionExtensions;

namespace AdventOfCode_2020_24;

public class Floor
{
    private readonly IDictionary<Point,int> _flipped;

    public Floor() : this(new Dictionary<Point, int>())
    {
    }

    public Floor(IEnumerable<Path> paths) : this(new Dictionary<Point, int>())
    {
        FlipEndOf(paths);
    }

    private Floor(IDictionary<Point, int> flipped)
    {
        _flipped = flipped;
    }

    public int BlackTiles => _flipped.Values.Count(IsBlack);
    public int WhiteTiles => _flipped.Values.Count(IsWhite);

    private static bool IsWhite(int flips) => flips % 2 == 0;
    private static bool IsBlack(int flips) => !IsWhite(flips);

    public bool IsBlack(Point p) => _flipped.ContainsKey(p) && IsBlack(_flipped[p]);
    public bool IsWhite(Point p) => !_flipped.ContainsKey(p) || IsWhite(_flipped[p]);

    private static IEnumerable<Point> PointsAround(Point p) => 
        Enum.GetValues<HexDirection>().
            Select(HexDirectionToVector).
            Select(vec => p.Add(vec));

    private void AddImplicitWhitesAroundBlacks()
    {
        var surroundingTiles = _flipped.
            Where(kv => IsBlack(kv.Value)).
            SelectMany(kv => PointsAround(kv.Key)).
            Distinct();

        var toBeAdded = surroundingTiles.Where(surroundingTile => !_flipped.ContainsKey(surroundingTile)).ToList();

        foreach (var point in toBeAdded)
        {
            _flipped.Add(point, 0);
        }
    }

    public Floor GetNextDay()
    {
        AddImplicitWhitesAroundBlacks();

        var newFlips = new Dictionary<Point, int>(_flipped);

        foreach (var (point,value) in _flipped)
        {
            if (IsBlack(value))
            {
                var adjacentBlackTiles = PointsAround(point).Count(IsBlack);
                if (adjacentBlackTiles is 0 or > 2) newFlips[point]++;
            }else{
                var adjacentBlackTiles = PointsAround(point).Count(IsBlack);
                if (adjacentBlackTiles is 2) newFlips[point]++;
            }
        }

        return new Floor(newFlips);
    }

    private void FlipEndOf(IEnumerable<Path> paths)
    {
        foreach (var path in paths)
        {
            FlipEndOf(path);
        }
    }

    public void FlipEndOf(Path path)
    {
        var endTile = path.EndCoordinate();

        if (!_flipped.ContainsKey(endTile))
        {
            _flipped.Add(endTile, 0);
        }
        _flipped[endTile]++;
    }
}