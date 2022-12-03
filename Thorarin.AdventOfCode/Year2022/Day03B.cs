using Thorarin.AdventOfCode.Extensions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 3, Part = 2)]
public class Day03B : Puzzle
{
    private string[] _rucksacks;

    public override void ParseInput(TextReader reader)
    {
        _rucksacks = reader.ToLineArray();
    }

    public override Output SampleExpectedOutput => 70;

    public override Output Run()
    {
        return _rucksacks
            .Chunk(3)
            .Select(group => CalculatePriority(group.IntersectAll().Single()))
            .Sum();
    }

    private static int CalculatePriority(char item) => char.IsUpper(item) ? item - 38 : item - 96;
}