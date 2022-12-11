using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 11, Part = 2)]
public class Day11B : Day11Base
{
    public override Output SampleExpectedOutput => 2_713_310_158;

    public override Output ProblemExpectedOutput => 14_081_365_540;

    public override Output Run()
    {
        var divisor = Monkeys.Select(m => m.Test).Distinct().Aggregate((a, b) => a * b);

        DoMonkeyBusinessRounds(10_000, worry => (int)(worry % divisor));

        return CalculateMonkeyBusinessLevel();
    }
}