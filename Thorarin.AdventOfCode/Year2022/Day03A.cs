using Thorarin.AdventOfCode.Extensions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 3, Part = 1)]
public class Day03A : Puzzle
{
    private string[] _rucksacks;

    public override void ParseInput(TextReader reader)
    {
        _rucksacks = reader.ToLineArray();
    }

    public override Output SampleExpectedOutput => 157;

    public override Output Run()
    {
        return _rucksacks
            .Select(r => CalculatePriority(r.Chunk(r.Length / 2).IntersectAll().Single()))
            .Sum();
    }

    private static int CalculatePriority(char item) => char.IsUpper(item) ? item - 38 : item - 96;
}