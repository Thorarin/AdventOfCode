using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

public class Day01B_Linq : Day01B
{
    public override Output Run()
    {
        int Solve(IReadOnlyList<int> source, int gap)
        {
            return source.Zip(source.Skip(gap)).Count(tuple => tuple.First < tuple.Second);
        }

        return Solve(Input, 3);
    }
}