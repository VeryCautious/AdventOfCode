namespace AdventOfCode_2020_23;

public static class Crab
{

    public static (ICircle<int> newCircle, int nextselectedElement) MakeAMoveOn(ICircle<int> circle, int selectedElement)
    {
        var circleSize = circle.Count;
        var newCircle = circle.TakeOutAfter(selectedElement, 3, out var removedCups);

        var target = selectedElement;
        do
        {
            target--;
            if (target <= 0)
            {
                target += circleSize;
            }
        } while (removedCups.Contains(target));

        newCircle = newCircle.InsertRangeAfter(target, removedCups);

        return (newCircle, newCircle.ElementAfter(selectedElement));
    }

    public static ICircle<int> MakeNMovesOn(ICircle<int> circle, int n)
    {
        return Enumerable.
            Repeat(MakeAMoveOn, n).
            Aggregate(
                (circle, circle.First()),
                (prev, func) => (func(prev.circle, prev.Item2))
            ).circle;
    }

}