using System.Collections.Immutable;

namespace AdventOfCode_2020_23;

public class ImmutableCircle<T> : ICircle<T>
{
    private readonly IImmutableList<T> _items;

    public ImmutableCircle(IEnumerable<T> items)
    {
        _items = items.ToImmutableList();
    }

    public IEnumerable<T> GetElements()
    {
        return _items;
    }

    public IEnumerable<T> GetElements(T startElement)
    {
        return GetElements(_items.IndexOf(startElement));
    }

    public int Count => _items.Count;

    public T ElementAfter(T element)
    {
        var indexOfNext = IndexAfter(_items.IndexOf(element));
        return _items[indexOfNext];
    }

    public ICircle<T> TakeOutAfter(T element, int amount, out T[] removedItems)
    {
        var newCircle = TakeOut(IndexAfter(_items.IndexOf(element)), amount, out var removedArray);
        removedItems = removedArray.ToArray();
        return newCircle;
    }

    public ICircle<T> InsertRangeAfter(T element, IEnumerable<T> items)
    {
        return InsertRangeAt(IndexAfter(_items.IndexOf(element)), items);
    }

    public T First()
    {
        return _items[0];
    }

    private IEnumerable<T> GetElements(int startIndex)
    {
        return _items.Skip(startIndex).Concat(_items.Take(startIndex));
    }

    private int IndexAfter(int index)
    {
        return (index + 1) % _items.Count;
    }

    private ImmutableCircle<T> TakeOut(int startIndex, int amount, out T[] removedItems)
    {
        if (startIndex + amount <= _items.Count)
        {
            removedItems = _items.Skip(startIndex).Take(amount).ToArray();
            return new ImmutableCircle<T>(_items.Take(startIndex).Concat(_items.Skip(startIndex + amount)));
        }

        var leftOver = startIndex + amount - _items.Count;

        removedItems = _items.Skip(startIndex).Take(amount - leftOver).Concat(_items.Take(leftOver)).ToArray();
        return new ImmutableCircle<T>(_items.Skip(leftOver).Take(startIndex - leftOver));
    }

    private ImmutableCircle<T> InsertRangeAt(int index, IEnumerable<T> items)
    {
        return new ImmutableCircle<T>(_items.Take(index).Concat(items).Concat(_items.Skip(index)));
    }
}