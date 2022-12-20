using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 20, Part = 2)]
public class Day20B : Day20Base
{
    public override Output SampleExpectedOutput => 1_623_178_306;

    public override Output ProblemExpectedOutput => 4_248_669_215_955;

    public override Output Run()
    {
        return  Mix(_input, 811589153, 10);
    }
}