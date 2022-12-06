
namespace Fax.Core.Tests.Collections;

public class ListExtensionsTests
{
    [Fact]
    public void SortByDependencies()
    {
        var dependencies = new Dictionary<char, char[]>
        {
            {'A', new[] {'B', 'G'}},
            {'B', new[] {'C', 'E'}},
            {'C', new[] {'D'}},
            {'D', new char[0]},
            {'E', new[] {'C', 'F'}},
            {'F', new[] {'C'}},
            {'G', new[] {'F'}}
        };

        for (int i = 0; i < 3; i++)
        {
            var list = RandomHelper
                .GenerateRandomizedList(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' });

            list = list.SortByDependencies(c => dependencies[c]);

            foreach (var dependency in dependencies)
            {
                foreach (var dependedValue in dependency.Value)
                {
                    list.IndexOf(dependency.Key).ShouldBeGreaterThan(list.IndexOf(dependedValue));
                }
            }
        }
    }
}