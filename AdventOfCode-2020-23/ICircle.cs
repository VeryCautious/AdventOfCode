using System.Collections.Immutable;

namespace AdventOfCode_2020_23;

public interface ICircle<T>
{
    IEnumerable<T> GetElements();
    IEnumerable<T> GetElements(T startElement);
    int Count { get; }
    T ElementAfter(T element);
    ICircle<T> TakeOutAfter(T element, int amount, out T[] removedItems);
    ICircle<T> InsertRangeAfter(T element, IEnumerable<T> items);
    T First();
}