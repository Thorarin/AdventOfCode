namespace Thorarin.AdventOfCode.Extensions
{
    public static class ReadOnlySpanExtensions
    {
        public static SplitEnumerator EnumerateSplit(this ReadOnlySpan<char> span, char splitChar)
        {
            ReadOnlyMemory<char> split = new[] { splitChar };
            return new SplitEnumerator(span, split.Span);
        }

        public static SplitEnumerator EnumerateSplit(this ReadOnlySpan<char> span, ReadOnlySpan<char> split)
        {
            return new SplitEnumerator(span, split);
        }
    }
}
