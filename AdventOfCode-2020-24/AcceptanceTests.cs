using System.Collections.Immutable;
using System.Drawing;
using FluentAssertions;
using Xunit;
using static AdventOfCode_2020_24.HexDirectionParser;

namespace AdventOfCode_2020_24;

public class AcceptanceTests
{
    private const string Example = 
@"sesenwnenenewseeswwswswwnenewsewsw
neeenesenwnwwswnenewnwwsewnenwseswesw
seswneswswsenwwnwse
nwnwneseeswswnenewneswwnewseswneseene
swweswneswnenwsewnwneneseenw
eesenwseswswnenwswnwnwsewwnwsene
sewnenenenesenwsewnenwwwse
wenwwweseeeweswwwnwwe
wsweesenenewnwwnwsenewsenwwsesesenwne
neeswseenwwswnwswswnw
nenwswwsewswnenenewsenwsenwnesesenew
enewnwewneswsewnwswenweswnenwsenwsw
sweneswneswneneenwnewenewwneswswnese
swwesenesewenwneswnwwneseswwne
enesenwswwswneneswsenwnewswseenwsese
wnwnesenesenenwwnenwsewesewsesesew
nenewswnwewswnenesenwnesewesw
eneswnwswnwsenenwnwnwwseeswneewsenese
neswnwewnwnwseenwseesewsenwsweewe
wseweeenwnesenwwwswnew";

    [Fact]
    public void EncodedLine_Parse_DecodedLine()
    {
        var expected = new[]
        {
            HexDirection.SouthEast,
            HexDirection.SouthWest,
            HexDirection.NorthEast,
            HexDirection.SouthWest,
            HexDirection.SouthWest,
            HexDirection.SouthEast, 
            HexDirection.NorthWest,
            HexDirection.West,
            HexDirection.NorthWest,
            HexDirection.SouthEast
        };
        const string input = "seswneswswsenwwnwse";

        var directions = Parse(input);

        directions.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Point_EndOfPath_SamePoint()
    {
        var expected = new Point(0,0);
        var directions = Parse("nwwswee");

        var path = new Path(directions);

        path.EndCoordinate().Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Paths_CreateFloor_10Black5White()
    {
        var paths = Example.
            Split("\n").
            Select(Parse).
            Select(directions => new Path(directions)).
            ToImmutableList();
        var floor = new Floor(paths);

        floor.BlackTiles.Should().Be(10);
        floor.WhiteTiles.Should().Be(5);
    }

    [Fact]
    public void Floor_GetNextDay_15BlackTiles()
    {
        var paths = Example.
            Split("\n").
            Select(Parse).
            Select(directions => new Path(directions)).
            ToImmutableList();
        var floor = new Floor(paths).GetNextDay();

        floor.BlackTiles.Should().Be(15);
    }

    [Fact]
    public void Floor_GetNextDay100Times_2208BlackTiles()
    {
        var paths = Example.
            Split("\n").
            Select(Parse).
            Select(directions => new Path(directions)).
            ToImmutableList();
        var floor = new Floor(paths);

        var floorAfter100Days = Enumerable.
            Range(1, 100).
            Aggregate(floor, (prev, _) => prev.GetNextDay());

        floorAfter100Days.BlackTiles.Should().Be(2208);
    }

    [Fact]
    public void Puzzle1()
    {
        var paths = File.ReadAllText("puzzle-input.txt").
            Split("\n").
            Select(Parse).
            Select(directions => new Path(directions)).
            ToImmutableList();
        var floor = new Floor();

        foreach (var path in paths)
        {
            floor.FlipEndOf(path);
        }

        floor.BlackTiles.Should().Be(436);
    }

    [Fact]
    public void Puzzle2()
    {
        var paths = File.ReadAllText("puzzle-input.txt").
            Split("\n").
            Select(Parse).
            Select(directions => new Path(directions)).
            ToImmutableList();
        var floor = new Floor(paths);

        var floorAfter100Days = Enumerable.
            Range(1, 100).
            Aggregate(floor, (prev, _) => prev.GetNextDay());

        floorAfter100Days.BlackTiles.Should().Be(4133);
    }
}