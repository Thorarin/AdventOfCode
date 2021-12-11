using Thorarin.AdventOfCode.Framework;
using MoreLinq;

namespace Thorarin.AdventOfCode.Year2021;

public class Day01B_MoreLinq : Day01B
{
    public override Output Run()
    {
        return Input
            .Window(3).Select(Enumerable.Sum)
            .Window(2).Count(w => w.First() < w.Last());
    }
}
