namespace Thorarin.AdventOfCode.Extensions
{
    internal static class HashSetExtensions
    {
        public static void AddRange<T>(this ISet<T> set, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                set.Add(item);
            }
        }

    }
}
