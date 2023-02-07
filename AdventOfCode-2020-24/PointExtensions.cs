using System.Drawing;

namespace AdventOfCode_2020_24;

public static class PointExtensions
{
    public static Point Add(this Point p1, Point p2) => new(p1.X + p2.X, p1.Y + p2.Y);
}