namespace AdventOfCode_2020_23;

public class InPlaceCircle<T> : ICircle<T> where T : notnull
{
    private readonly T _first;
    private readonly IDictionary<T, T> _next;

    public InPlaceCircle(IEnumerable<T> items)
    {
        var array = items as T[] ?? items.ToArray();
        _first = array[0];
        _next = new Dictionary<T, T>(array.Length);
        foreach (var (item, index) in array.Select((item, index) => (item, index)))
            _next.Add(item, array[(index + 1) % array.Length]);
    }

    public IEnumerable<T> GetElements()
    {
        return GetElements(_next.Keys.First());
    }

    public IEnumerable<T> GetElements(T startElement)
    {
        yield return startElement;
        var next = _next[startElement];

        for (var i = 1; i < _next.Count; i++)
        {
            yield return next;
            next = _next[next];
        }
    }

    public int Count => _next.Count;

    public T ElementAfter(T element)
    {
        return _next[element];
    }

    public ICircle<T> TakeOutAfter(T element, int amount, out T[] removedItems)
    {
        var removed = new T[amount];
        removed[0] = _next[element];

        for (var i = 1; i < amount; i++) removed[i] = _next[removed[i - 1]];

        _next[element] = _next[removed[amount - 1]];
        removedItems = removed;
        return this;
    }

    public ICircle<T> InsertRangeAfter(T element, IEnumerable<T> items)
    {
        var veryLast = _next[element];
        var last = element;

        foreach (var item in items)
        {
            _next[last] = item;
            last = item;
        }

        _next[last] = veryLast;

        return this;
    }

    public T First()
    {
        return _first;
    }

    public override string ToString()
    {
        return string.Join(",", GetElements());
    }
}