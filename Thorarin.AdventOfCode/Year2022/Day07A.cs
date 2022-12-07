using MoreLinq;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 7, Part = 1)]
public class Day07A : Day07Base
{
    private string[] _lines;

    public override void ParseInput(TextReader reader)
    {
        _lines = reader.ToLineArray();
    }

    public override Output SampleExpectedOutput => 95437;

    public override Output ProblemExpectedOutput => 1447046;

    public override Output Run()
    {
        var directories = TraverseDirectories(_lines);

        return directories.Where(d => d.Size <= 100_000).Sum(d => d.Size);
    }
}