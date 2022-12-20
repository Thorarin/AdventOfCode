using System.Diagnostics;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 20, Part = 1)]
public class Day20A : Day20Base
{
    public override Output SampleExpectedOutput => 3;

    // 8401 too low
    public override Output ProblemExpectedOutput => 11123;

    public override Output Run()
    {
        return Mix(_input, 1, 1);
    }
}