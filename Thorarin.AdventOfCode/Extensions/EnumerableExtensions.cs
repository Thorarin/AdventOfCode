namespace Thorarin.AdventOfCode.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Intersect multiple sequences (to find the elements common to each sequence).
        /// </summary>
        public static IEnumerable<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            return sequences.Aggregate((acc, next) => acc.Intersect(next));
        }
    }
}
