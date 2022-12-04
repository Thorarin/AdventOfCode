using Thorarin.AdventOfCode.Framework;
using Range = Thorarin.AdventOfCode.Year2022.Day04.Range;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 4, Part = 1)]
public class Day04A : Puzzle
{
    private readonly List<List<Range>> _sections = new();

    public override void ParseInput(TextReader reader)
    {
        foreach (var line in reader.AsLines())
        {
            _sections.Add(line.Split(',').Select(Range.Parse).ToList());
        }
    }

    public override Output SampleExpectedOutput => 2;

    public override Output Run()
    {
        return _sections.Count(x => x[0].FullyContainedIn(x[1]) || x[1].FullyContainedIn(x[0]));
    }
}