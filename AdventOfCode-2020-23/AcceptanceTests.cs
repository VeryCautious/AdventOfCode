using FluentAssertions;
using Xunit;
using static AdventOfCode_2020_23.Crab;

namespace AdventOfCode_2020_23;

public class AcceptanceTests
{

    [Fact]
    public void ImmutableCircle_TakOutAfterNotWrapping_NewCircle()
    {
        var expected = new ImmutableCircle<int>(new[] { 1, 2 });
        var circle = new ImmutableCircle<int>(new[] { 1, 2, 3, 4, 5 });

        var actual = circle.TakeOutAfter(2, 3, out var removedItems);

        actual.GetElements().Should().ContainInConsecutiveOrder(expected.GetElements());
        removedItems.Should().ContainInConsecutiveOrder(new[]{ 3, 4, 5 });
    }

    [Fact]
    public void ImmutableCircle_TakOutAfterWrapping_NewCircle()
    {
        var expected = new ImmutableCircle<int>(new[] { 3, 4 });
        var circle = new ImmutableCircle<int>(new[] { 1, 2, 3, 4, 5 });

        var actual = circle.TakeOutAfter(4, 3, out var removedItems);

        actual.GetElements().Should().ContainInConsecutiveOrder(expected.GetElements());
        removedItems.Should().ContainInConsecutiveOrder(new[]{ 5, 1, 2 });
    }

    [Fact]
    public void ImmutableCircle_TakOutAfterWrapping2_NewCircle()
    {
        var expected = new ImmutableCircle<int>(new[] { 2, 3 });
        var circle = new ImmutableCircle<int>(new[] { 1, 2, 3, 4, 5 });

        var actual = circle.TakeOutAfter(3, 3, out var removedItems);

        actual.GetElements().Should().ContainInConsecutiveOrder(expected.GetElements());
        removedItems.Should().ContainInConsecutiveOrder(new[]{ 4, 5, 1 });
    }

    [Fact]
    public void ImmutableCircle_TakOutAfterOnlyWrapping_NewCircle()
    {
        var expected = new ImmutableCircle<int>(new[] { 3, 4, 5 });
        var circle = new ImmutableCircle<int>(new[] { 1, 2, 3, 4, 5 });

        var actual = circle.TakeOutAfter(5, 2, out var removedItems);

        actual.GetElements().Should().ContainInConsecutiveOrder(expected.GetElements());
        removedItems.Should().ContainInConsecutiveOrder(new[]{ 1, 2 });
    }
    
    [Fact]
    public void ImmutableCircle_InsertAfter_NewCircle()
    {
        var expected = new ImmutableCircle<int>(new[] { 1, 2, 3, 6, 7, 4, 5 });
        var circle = new ImmutableCircle<int>(new[] { 1, 2, 3, 4, 5 });

        var actual = circle.InsertRangeAfter(3, new []{ 6, 7 });

        actual.GetElements().Should().ContainInConsecutiveOrder(expected.GetElements());
    }

    [Fact]
    public void ImmutableCircle_InsertAfterWrapping_NewCircle()
    {
        var expected = new ImmutableCircle<int>(new[] { 6, 7, 1, 2, 3, 4, 5 });
        var circle = new ImmutableCircle<int>(new[] { 1, 2, 3, 4, 5 });

        var actual = circle.InsertRangeAfter(5, new []{ 6, 7 });

        actual.GetElements().Should().ContainInConsecutiveOrder(expected.GetElements());
    }

    [Fact]
    public void ImmutableCircle_MakeAMove_NewCircle()
    {
        var expected = new ImmutableCircle<int>(new[] { 3, 2, 8, 9, 1, 5, 4, 6, 7 });
        var circle = new ImmutableCircle<int>(new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 });

        var actual = MakeAMoveOn(circle, circle.First()).newCircle;

        actual.GetElements().Should().ContainInConsecutiveOrder(expected.GetElements());
    }

    [Fact]
    public void ImmutableCircle_Make10Move_NewCircle()
    {
        var expected = new ImmutableCircle<int>(new[] { 5, 8, 3, 7, 4, 1, 9, 2, 6 });
        var circle = new ImmutableCircle<int>(new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 });

        var actual = MakeNMovesOn(circle, 10);

        actual.GetElements(1).Should().ContainInConsecutiveOrder(expected.GetElements(1));
    }

    [Fact]
    public void ImmutableCircle_Make10MoveAndGetElementsAfter1_Elements()
    {
        var expected = new[] { 9, 2, 6, 5, 8, 3, 7, 4 };
        var circle = new ImmutableCircle<int>(new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 });

        var resultCircle = MakeNMovesOn(circle, 10);
        var actual = resultCircle.GetElements(1).Skip(1).ToArray();

        actual.Should().ContainInConsecutiveOrder(expected);
    }

    [Fact]
    public void ImmutableCircle_Make100MoveAndGetElementsAfter1_Elements()
    {
        const string expected = "67384529";
        var circle = new ImmutableCircle<int>(new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 });

        var resultCircle = MakeNMovesOn(circle, 100);
        var elements = resultCircle.
            GetElements(1).
            Skip(1).
            Select(number => number.ToString());

        var actual = string.Join("", elements);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void InPlaceCircle_TakOutAfterNotWrapping_Removed()
    {
        var expected = new InPlaceCircle<int>(new[] { 1, 2 });
        var circle = new InPlaceCircle<int>(new[] { 1, 2, 3, 4, 5 });

        var actual = circle.TakeOutAfter(2, 3, out var removedItems);

        actual.GetElements().Should().ContainInConsecutiveOrder(expected.GetElements());
        removedItems.Should().ContainInConsecutiveOrder(new[]{ 3, 4, 5 });
    }

    [Fact]
    public void InPlaceCircle_TakOutAfterWrapping_NewCircle()
    {
        var expected = new InPlaceCircle<int>(new[] { 3, 4 });
        var circle = new InPlaceCircle<int>(new[] { 1, 2, 3, 4, 5 });

        var actual = circle.TakeOutAfter(4, 3, out var removedItems);

        actual.GetElements().Should().ContainInConsecutiveOrder(expected.GetElements());
        removedItems.Should().ContainInConsecutiveOrder(new[]{ 5, 1, 2 });
    }

    [Fact]
    public void InPlaceCircle_TakOutAfterWrapping2_NewCircle()
    {
        var expected = new InPlaceCircle<int>(new[] { 2, 3 });
        var circle = new InPlaceCircle<int>(new[] { 1, 2, 3, 4, 5 });

        var actual = circle.TakeOutAfter(3, 3, out var removedItems);

        actual.GetElements().Should().ContainInConsecutiveOrder(expected.GetElements());
        removedItems.Should().ContainInConsecutiveOrder(new[]{ 4, 5, 1 });
    }

    [Fact]
    public void InPlaceCircle_TakOutAfterOnlyWrapping_NewCircle()
    {
        var expected = new InPlaceCircle<int>(new[] { 3, 4, 5 });
        var circle = new InPlaceCircle<int>(new[] { 1, 2, 3, 4, 5 });

        var actual = circle.TakeOutAfter(5, 2, out var removedItems);

        actual.GetElements().Should().ContainInConsecutiveOrder(expected.GetElements());
        removedItems.Should().ContainInConsecutiveOrder(new[]{ 1, 2 });
    }
    
    [Fact]
    public void InPlaceCircle_InsertAfter_NewCircle()
    {
        var expected = new InPlaceCircle<int>(new[] { 1, 2, 3, 6, 7, 4, 5 });
        var circle = new InPlaceCircle<int>(new[] { 1, 2, 3, 4, 5 });

        var actual = circle.InsertRangeAfter(3, new []{ 6, 7 });

        actual.GetElements().Should().ContainInConsecutiveOrder(expected.GetElements());
    }

    [Fact]
    public void InPlaceCircle_InsertAfterWrapping_NewCircle()
    {
        var expected = new InPlaceCircle<int>(new[] { 6, 7, 1, 2, 3, 4, 5 });
        var circle = new InPlaceCircle<int>(new[] { 1, 2, 3, 4, 5 });

        var actual = circle.InsertRangeAfter(5, new []{ 6, 7 });

        actual.GetElements(1).Should().ContainInConsecutiveOrder(expected.GetElements(1));
    }

    [Fact]
    public void InPlaceCircle_MakeAMove_NewCircle()
    {
        var expected = new InPlaceCircle<int>(new[] { 3, 2, 8, 9, 1, 5, 4, 6, 7 });
        var circle = new InPlaceCircle<int>(new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 });

        var actual = MakeAMoveOn(circle, circle.First()).newCircle;

        actual.GetElements(1).Should().ContainInConsecutiveOrder(expected.GetElements(1));
    }

    [Fact]
    public void InPlaceCircle_Make10Move_NewCircle()
    {
        var expected = new InPlaceCircle<int>(new[] { 5, 8, 3, 7, 4, 1, 9, 2, 6 });
        var circle = new InPlaceCircle<int>(new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 });

        var actual = MakeNMovesOn(circle, 10);

        actual.GetElements(1).Should().ContainInConsecutiveOrder(expected.GetElements(1));
    }

    [Fact]
    public void InPlaceCircle_Make10MoveAndGetElementsAfter1_Elements()
    {
        var expected = new[] { 9, 2, 6, 5, 8, 3, 7, 4 };
        var circle = new InPlaceCircle<int>(new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 });

        var resultCircle = MakeNMovesOn(circle, 10);
        var actual = resultCircle.GetElements(1).Skip(1).ToArray();

        actual.Should().ContainInConsecutiveOrder(expected);
    }

    [Fact]
    public void InPlaceCircle_Make100MoveAndGetElementsAfter1_Elements()
    {
        const string expected = "67384529";
        var circle = new InPlaceCircle<int>(new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 });

        var resultCircle = MakeNMovesOn(circle, 100);
        var elements = resultCircle.
            GetElements(1).
            Skip(1).
            Select(number => number.ToString());

        var actual = string.Join("", elements);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Puzzle1()
    {
        const string expected = "89573246";
        var circle = new ImmutableCircle<int>(new[] { 4, 8, 7, 9, 1, 2, 3, 6, 5 });

        var resultCircle = MakeNMovesOn(circle, 100);
        var elements = resultCircle.
            GetElements(1).
            Skip(1).
            Select(number => number.ToString());

        var actual = string.Join("", elements);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Example2()
    {
        const string expected = "149245887792";
        var circle = new InPlaceCircle<int>(new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 }.Concat(Enumerable.Range(10, 1000000-9)));

        var resultCircle = MakeNMovesOn(circle, 10000000);
        var elements = resultCircle.
            GetElements(1).
            Skip(1).
            Take(2).
            Select(number => (long)number).
            Aggregate((long)1, (number,prev) => number * prev).
            ToString();

        var actual = string.Join("", elements);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Puzzle2()
    {
        const string expected = "2029056128";
        var circle = new InPlaceCircle<int>(new[] { 4, 8, 7, 9, 1, 2, 3, 6, 5 }.Concat(Enumerable.Range(10, 1000000-9)));

        var resultCircle = MakeNMovesOn(circle, 10000000);
        var elements = resultCircle.
            GetElements(1).
            Skip(1).
            Take(2).
            Select(number => (long)number).
            Aggregate((long)1, (number,prev) => number * prev).
            ToString();

        var actual = string.Join("", elements);

        actual.Should().BeEquivalentTo(expected);
    }
}