using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 11, Part = 1)]
public class Day11A : Day11Base
{
    public override Output SampleExpectedOutput => 10605;

    public override Output ProblemExpectedOutput => 61503;

    public override Output Run()
    {
        DoMonkeyBusinessRounds(20, worry => (int)worry / 3);

        return CalculateMonkeyBusinessLevel();
    }
}