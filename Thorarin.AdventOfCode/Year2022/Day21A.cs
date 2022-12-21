using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 21, Part = 1)]
public class Day21A : Day21Base
{
    public override Output SampleExpectedOutput => 152;

    public override Output ProblemExpectedOutput => 159_591_692_827_554;

    public override Output Run()
    {
        return GetMonkey("root").GetValue();
    }
}