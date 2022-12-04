using Thorarin.AdventOfCode.Framework;
using Range = Thorarin.AdventOfCode.Year2022.Day04.Range;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 4, Part = 2)]
public class Day04B : Puzzle
{
    private readonly List<List<Range>> _sections = new();

    public override void ParseInput(TextReader reader)
    {
        foreach (var line in reader.AsLines())
        {
            _sections.Add(line.Split(',').Select(Range.Parse).ToList());
        }
    }

    public override Output SampleExpectedOutput => 4;

    public override Output Run()
    {
        return _sections.Count(x => x[0].Intersect(x[1]).HasValue);
    }
    
}