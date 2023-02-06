namespace AdventOfCode_2020_21.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> sets)
    {
        var enumerable = sets as IEnumerable<T>[] ?? sets.ToArray();

        if (!enumerable.Any()) return Enumerable.Empty<T>();

        return enumerable.First().Where(item => enumerable.All(set => set.Contains(item)));
    }
}