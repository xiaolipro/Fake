namespace Fax.Core.Collections.Generic;

public static class ListExtensions
{
    /// <summary>
    /// Sort a list by a topological sorting, which consider their dependencies.
    /// </summary>
    /// <typeparam name="T">The type of the members of values.</typeparam>
    /// <param name="source">A list of objects to sort</param>
    /// <param name="getDependencies">Function to resolve the dependencies</param>
    /// <param name="comparer">Equality comparer for dependencies </param>
    /// <returns>
    /// Returns a new list ordered by dependencies.
    /// If A depends on B, then B will come before than A in the resulting list.
    /// </returns>
    public static List<T> SortByDependencies<T>(
        this IEnumerable<T> source,
        Func<T, IEnumerable<T>> getDependencies,
        IEqualityComparer<T> comparer = null)
    {
        /* See: http://www.codeproject.com/Articles/869059/Topological-sorting-in-Csharp
         *      http://en.wikipedia.org/wiki/Topological_sorting
         */

        var sorted = new List<T>();
        
        // 维护一个访问hash
        var visited = new Dictionary<T, bool>(comparer);

        foreach (var item in source)
        {
            SortByDependenciesVisit(item, getDependencies, sorted, visited);
        }

        return sorted;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T">The type of the members of values.</typeparam>
    /// <param name="item">Item to resolve</param>
    /// <param name="getDependencies">Function to resolve the dependencies</param>
    /// <param name="sorted">List with the sorted items</param>
    /// <param name="visited">Dictionary with the visited items</param>
    private static void SortByDependenciesVisit<T>(T item, Func<T, IEnumerable<T>> getDependencies, List<T> sorted,
        Dictionary<T, bool> visited)
    {
        var alreadyVisited = visited.TryGetValue(item, out var processing);

        // 如果已经访问过了，并且还在处理中，则出现循环引用
        if (alreadyVisited && processing)
        {
            throw new ArgumentException("Cyclic dependency found! Item: " + item);
        }

        visited[item] = true;

        var dependencies = getDependencies(item);
        if (dependencies != null)
        {
            foreach (var dependency in dependencies)
            {
                SortByDependenciesVisit(dependency, getDependencies, sorted, visited);
            }
        }

        visited[item] = false;
        sorted.Add(item);
    }
}