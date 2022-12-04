namespace Thorarin.AdventOfCode.Year2022.Day04;

internal readonly record struct Range(int From, int To)
{
    public Range? Intersect(Range other)
    {
        var from = Math.Max(From, other.From);
        var to = Math.Min(To, other.To);

        if (from <= to) return new Range(from, to);
        return null;
    }

    public bool FullyContainedIn(Range other)
    {
        var intersection = Intersect(other);
        return intersection.HasValue && intersection.Value == this;
    }

    public static Range Parse(string range)
    {
        var split = range.Split('-');
        return new Range(int.Parse(split[0]), int.Parse(split[1]));
    }
}