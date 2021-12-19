namespace Thorarin.AdventOfCode.Year2021.Day19;

public static class Rotation
{
    public static IEnumerable<IReadOnlyList<Point>> Rotate(this IReadOnlyList<Point> points)
    {
        for (int v = 0; v < 24; v++)
        {
            yield return points.Select(p => p.Variation(v)).ToList();
        }
    }
}